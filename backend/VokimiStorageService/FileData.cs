namespace VokimiStorageService;

public record class FileData(
    Stream Stream,
    string ContentType
);