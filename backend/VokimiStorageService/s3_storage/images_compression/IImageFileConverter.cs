namespace VokimiStorageService.s3_storage.images_compression;

internal interface IImageFileConverter
{
    Task<ErrOr<ImageFileAfterCompression>> CompressAsync(FileData file, CancellationToken ct);
}
public sealed record ImageFileAfterCompression(
    Stream Stream,
    ImageFileExtension Extension,
    string ContentType,
    bool Changed,
    long OriginalBytes,
    long ResultBytes
);