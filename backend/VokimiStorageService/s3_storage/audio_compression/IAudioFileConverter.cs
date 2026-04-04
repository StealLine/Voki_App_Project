namespace VokimiStorageService.s3_storage.audio_compression;

public interface IAudioFileConverter
{
    Task<ErrOr<AudioFileAfterConversion>> ConvertAudioAsync(FileData data, CancellationToken ct);
}

public record AudioFileAfterConversion(
    Stream Stream,
    string ContentType,
    string Extension,
    bool Changed,
    long OriginalBytes,
    long ResultBytes
);
