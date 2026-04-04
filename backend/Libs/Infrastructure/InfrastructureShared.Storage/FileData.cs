namespace InfrastructureShared.Storage;

public record class FileData(
    Stream Stream,
    string ContentType
);