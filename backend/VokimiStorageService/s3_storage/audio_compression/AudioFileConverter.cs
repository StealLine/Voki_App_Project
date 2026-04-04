using Xabe.FFmpeg;

namespace VokimiStorageService.s3_storage.audio_compression;

internal sealed class AudioFileConverter : IAudioFileConverter
{
    private readonly ILogger<AudioFileConverter> _logger;

    public AudioFileConverter(ILogger<AudioFileConverter> logger, IConfiguration configuration) {
        FFmpeg.SetExecutablesPath(configuration["FfmpegPath"]);
        _logger = logger;
    }

    public async Task<ErrOr<AudioFileAfterConversion>> ConvertAudioAsync(FileData data, CancellationToken ct) {
        string inputTempPath = Path.GetTempFileName();
        try {
            long originalSizeBytes = data.Stream.Length;

            if (data.Stream.CanSeek) {
                data.Stream.Position = 0;
            }

            await using (var fileStream = new FileStream(inputTempPath, FileMode.Create, FileAccess.Write)) {
                await data.Stream.CopyToAsync(fileStream, ct);
            }

            var mediaInfo = await FFmpeg.GetMediaInfo(inputTempPath, ct);
            var audioStream = mediaInfo.AudioStreams.FirstOrDefault();

            if (audioStream == null) {
                return ErrFactory.Conflict("No audio stream found in the uploaded file.");
            }

            var extOrErr = GetExtensionFromContentType(data.ContentType);
            if (extOrErr.IsErr(out var err)) {
                return err;
            }

            var decisionOrErr = AudioCompressionPolicy.Decide(originalSizeBytes, extOrErr.AsSuccess(), mediaInfo.Duration);
            if (decisionOrErr.IsErr(out err)) {
                _logger.LogWarning("ConvertAudioAsync: Policy rejected file. Reason: {Reason}", err.Message);
                return err;
            }

            AudioCompressionDecision decision = decisionOrErr.AsSuccess();
            _logger.LogInformation("ConvertAudioAsync: Policy decision: {Reason}", decision.Reason);

            if (!decision.ShouldTranscode || decision.TargetFormat == TargetAudioFormat.Passthrough) {
                if (data.Stream.CanSeek) {
                    data.Stream.Position = 0;
                }

                return new AudioFileAfterConversion(
                    Stream: data.Stream,
                    ContentType: data.ContentType,
                    Extension: extOrErr.AsSuccess(),
                    Changed: false,
                    OriginalBytes: originalSizeBytes,
                    ResultBytes: originalSizeBytes
                );
            }

            string outputExt = decision.TargetFormat == TargetAudioFormat.M4A ? ".m4a" : ".mp3";
            string outputTempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + outputExt);

            var conversion = FFmpeg.Conversions.New()
                .AddStream(audioStream)
                .SetOutput(outputTempPath);

            if (decision.TargetFormat == TargetAudioFormat.M4A) {
                audioStream.SetCodec(AudioCodec.aac);
            }

            if (decision.Bitrate.HasValue) {
                conversion.SetAudioBitrate(decision.Bitrate.Value * 1000);
            }

            await conversion.Start(ct);

            var convertedStream = new FileStream(
                outputTempPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096,
                FileOptions.DeleteOnClose
            );
            long resultBytes = convertedStream.Length;

            _logger.LogInformation("ConvertAudioAsync: FFmpeg conversion successful. Result Size: {Bytes} B", resultBytes);

            return new AudioFileAfterConversion(
                Stream: convertedStream,
                ContentType: decision.TargetFormat == TargetAudioFormat.M4A ? "audio/mp4" : "audio/mpeg",
                Extension: decision.TargetFormat == TargetAudioFormat.M4A ? "m4a" : "mp3",
                Changed: true,
                OriginalBytes: originalSizeBytes,
                ResultBytes: resultBytes
            );
        }
        catch (Exception ex) {
            _logger.LogError(ex, "ConvertAudioAsync: Error converting audio.");
            return ErrFactory.Conflict("Failed to convert audio file format.");
        }
        finally {
            if (File.Exists(inputTempPath)) {
                File.Delete(inputTempPath);
            }
        }
    }

    private static ErrOr<string> GetExtensionFromContentType(string contentType) {
        var ct = contentType.ToLowerInvariant();
        if (ct.Contains("mpeg") || ct.Contains("mp3")) return "mp3";
        if (ct.Contains("mp4") || ct.Contains("m4a")) return "m4a";
        if (ct.Contains("wav")) return "wav";
        if (ct.Contains("ogg")) return "ogg";
        if (ct.Contains("webm")) return "webm";

        return ErrFactory.Conflict($"Unsupported audio content type '{contentType}'");
    }
}