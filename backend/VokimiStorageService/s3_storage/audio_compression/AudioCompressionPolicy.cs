namespace VokimiStorageService.s3_storage.audio_compression;

internal static class AudioCompressionPolicy
{
    private const long MB = 1024L * 1024L;
    private static readonly TimeSpan MaxDuration = TimeSpan.FromMinutes(10);
    private const long MaxSizeBytes = 15 * MB;

    public static ErrOr<AudioCompressionDecision> Decide(long sizeBytes, string ext, TimeSpan duration) {
        if (duration > MaxDuration) {
            return ErrFactory.Conflict($"Audio duration {duration.TotalMinutes:F1} min exceeds limit of 10 min.");
        }

        if (sizeBytes > MaxSizeBytes) {
            return ErrFactory.Conflict($"Audio size {sizeBytes / (double)MB:F1} MB exceeds limit of 15 MB.");
        }

        bool isMp3 = ext.Equals("mp3", StringComparison.OrdinalIgnoreCase) ||
                     ext.Equals("mpeg", StringComparison.OrdinalIgnoreCase);

        if (isMp3 && sizeBytes <= 1 * MB) {
            return new AudioCompressionDecision(
                ShouldTranscode: false,
                TargetFormat: TargetAudioFormat.Passthrough,
                Bitrate: null,
                Reason: "Small MP3 (<= 1 MB), passthrough"
            );
        }

        int bitrate = GetBitrate(sizeBytes);
        return new AudioCompressionDecision(
            ShouldTranscode: true,
            TargetFormat: TargetAudioFormat.M4A,
            Bitrate: bitrate,
            Reason: $"Transcode to m4a with {bitrate} kbps (Size: {sizeBytes / (double)MB:F1} MB)"
        );
    }

    private static int GetBitrate(long sizeBytes) =>
        sizeBytes switch {
            <= 5 * MB => 192,
            <= 7 * MB => 160,
            <= 10 * MB => 128,
            _ => 96
        };
}