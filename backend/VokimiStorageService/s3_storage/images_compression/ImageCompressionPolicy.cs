namespace VokimiStorageService.s3_storage.images_compression;

internal static class ImageCompressionPolicy
{
    const long MB = 1024L * 1024L;

    public static ErrOr<ImageCompressionDecision> Decide(long sizeBytes, string ext) =>
        ext switch {
            "png" => DecidePng(sizeBytes),
            "jpg" or "jpeg" => DecideJpeg(sizeBytes),
            "webp" => DecideWebp(sizeBytes),
            _ => ErrFactory.Conflict($"Unsupported image format '{ext}'")
        };

    private static ErrOr<ImageCompressionDecision> DecidePng(long sizeBytes) {
        if (sizeBytes <= 3 * MB) {
            return new ImageCompressionDecision(
                ShouldTranscode: true,
                TargetFormat: TargetImageFormat.WebpLossless,
                Quality: null,
                Reason: "PNG -> WebP lossless (≤ 3 MB)"
            );
        }

        int q = QualityForLarge(sizeBytes); // 3–5–7–10–15
        return new ImageCompressionDecision(
            ShouldTranscode: true,
            TargetFormat: TargetImageFormat.WebpLossy,
            Quality: q,
            Reason: $"PNG -> WebP lossy q={q} (> 3 MB)"
        );
    }

    private static ErrOr<ImageCompressionDecision> DecideJpeg(long sizeBytes) {
        int? q = QualityForLossyWithSmallNoop(sizeBytes); // ≤1MB no-op
        if (q is null) {
            return new ImageCompressionDecision(
                ShouldTranscode: false,
                TargetFormat: TargetImageFormat.Passthrough,
                Quality: null,
                Reason: "Small JPEG (≤ 1 MB), passthrough"
            );
        }

        return new ImageCompressionDecision(
            ShouldTranscode: true,
            TargetFormat: TargetImageFormat.Jpeg,
            Quality: q.Value,
            Reason: $"JPEG re-encode q={q.Value}"
        );
    }

    private static ErrOr<ImageCompressionDecision> DecideWebp(long sizeBytes) {
        int? q = QualityForLossyWithSmallNoop(sizeBytes); // ≤1MB no-op
        if (q is null) {
            return new ImageCompressionDecision(
                ShouldTranscode: false,
                TargetFormat: TargetImageFormat.Passthrough,
                Quality: null,
                Reason: "Small WebP (≤ 1 MB), passthrough"
            );
        }

        return new ImageCompressionDecision(
            ShouldTranscode: true,
            TargetFormat: TargetImageFormat.WebpLossy,
            Quality: q.Value,
            Reason: $"WebP re-encode q={q.Value}"
        );
    }

    private static int QualityForLarge(long sizeBytes) =>
        sizeBytes switch {
            <= 5 * MB => 85,
            <= 7 * MB => 80,
            <= 10 * MB => 75,
            <= 15 * MB => 70,
            _ => 65
        };

    private static int? QualityForLossyWithSmallNoop(long sizeBytes) =>
        sizeBytes <= 1 * MB
            ? null // no-op
            : QualityForLarge(sizeBytes);
}