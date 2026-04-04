namespace VokimiStorageService.s3_storage.images_compression;

internal sealed record ImageCompressionDecision(
    bool ShouldTranscode,
    TargetImageFormat TargetFormat,
    int? Quality, // 1..100 for lossy; null for lossless
    string Reason
);

internal enum TargetImageFormat
{
    Passthrough,
    WebpLossless,
    WebpLossy,
    Jpeg
}