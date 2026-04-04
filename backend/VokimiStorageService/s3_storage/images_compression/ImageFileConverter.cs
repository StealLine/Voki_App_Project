using Xabe.FFmpeg;

namespace VokimiStorageService.s3_storage.images_compression;

internal sealed class ImageFileConverter : IImageFileConverter
{
    private readonly ILogger<ImageFileConverter> _logger;

    public ImageFileConverter(ILogger<ImageFileConverter> logger, IConfiguration configuration) {
        FFmpeg.SetExecutablesPath(configuration["FfmpegPath"]);
        _logger = logger;
    }

    public async Task<ErrOr<ImageFileAfterCompression>> CompressAsync(
        FileData file, CancellationToken ct
    ) {
        string inputTempPath = Path.GetTempFileName();
        try {
            var extOrErr = GetExtensionFromContentType(file.ContentType);
            if (extOrErr.IsErr(out var err)) {
                return err;
            }

            string ext = extOrErr.AsSuccess();
            var originalBytes = file.Stream.Length;

            var decisionOrErr = ImageCompressionPolicy.Decide(originalBytes, ext);
            if (decisionOrErr.IsErr(out err)) {
                return err;
            }

            ImageCompressionDecision decision = decisionOrErr.AsSuccess();
            file.Stream.Position = 0;

            // if passthrough return as is
            if (!decision.ShouldTranscode || decision.TargetFormat == TargetImageFormat.Passthrough) {
                return new ImageFileAfterCompression(
                    Stream: file.Stream,
                    Extension: ImageFileExtension.Create(ext).AsSuccess(),
                    ContentType: file.ContentType,
                    Changed: false,
                    OriginalBytes: originalBytes,
                    ResultBytes: originalBytes
                );
            }

            await using (var fileStream = new FileStream(inputTempPath, FileMode.Create, FileAccess.Write)) {
                await file.Stream.CopyToAsync(fileStream, ct);
            }

            string outputExt = decision.TargetFormat switch {
                TargetImageFormat.Jpeg => ".jpg",
                _ => ".webp"
            };
            string outputTempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + outputExt);

            _logger.LogInformation(
                "CompressAsync: Starting FFmpeg image conversion to {Ext} (Original Size: {Bytes} B). Reason: {Reason}",
                outputExt, originalBytes, decision.Reason);

            var conversion = FFmpeg.Conversions.New()
                .AddParameter($"-i \"{inputTempPath}\"")
                .SetOutput(outputTempPath);

            if (decision.TargetFormat == TargetImageFormat.Jpeg) {
                // FFmpeg -q:v 1-31 (1 is best). Mapping 1-100 to 31-1.
                int q = 31 - (int)((decision.Quality ?? 85) * 30 / 100.0);
                q = Math.Clamp(q, 1, 31);
                conversion.AddParameter($"-q:v {q}");
            }
            else if (decision.TargetFormat == TargetImageFormat.WebpLossless) {
                conversion.AddParameter("-lossless 1");
            }
            else if (decision.TargetFormat == TargetImageFormat.WebpLossy) {
                conversion.AddParameter($"-quality {decision.Quality ?? 80}");
            }

            await conversion.Start(ct);

            var convertedStream = new FileStream(
                outputTempPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096,
                FileOptions.DeleteOnClose
            );
            long resultBytes = convertedStream.Length;

            _logger.LogInformation("CompressAsync: FFmpeg conversion successful. Result Size: {Bytes} B", resultBytes);

            string contentType = decision.TargetFormat switch {
                TargetImageFormat.Jpeg => "image/jpeg",
                _ => "image/webp"
            };

            return new ImageFileAfterCompression(
                Stream: convertedStream,
                Extension: ToExtensionFromTarget(decision.TargetFormat),
                ContentType: contentType,
                Changed: true,
                OriginalBytes: originalBytes,
                ResultBytes: resultBytes
            );
        }
        catch (Exception ex) {
            _logger.LogError(ex, "CompressAsync: Error converting image.");
            return ErrFactory.Conflict($"Compression failed: {ex.Message}");
        }
        finally {
            if (File.Exists(inputTempPath)) {
                File.Delete(inputTempPath);
            }
        }
    }

    private static ImageFileExtension ToExtensionFromTarget(TargetImageFormat target) =>
        target switch {
            TargetImageFormat.Jpeg => ImageFileExtension.Jpeg,
            TargetImageFormat.WebpLossless or TargetImageFormat.WebpLossy => ImageFileExtension.Webp,
            _ => throw new ArgumentException($"Unsupported target {target}")
        };

    private static ErrOr<string> GetExtensionFromContentType(string contentType) =>
        contentType.ToLowerInvariant() switch {
            "image/png" => "png",
            "image/jpeg" or "image/jpg" => "jpeg",
            "image/webp" => "webp",
            _ => ErrFactory.Conflict($"Unsupported content type '{contentType}'")
        };
}