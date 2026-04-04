namespace VokimiStorageService.s3_storage.audio_compression;

internal sealed record AudioCompressionDecision(
    bool ShouldTranscode,
    TargetAudioFormat TargetFormat,
    int? Bitrate, // in kbps (192, 128, ...)
    string Reason
);

internal enum TargetAudioFormat
{
    Passthrough,
    M4A
}