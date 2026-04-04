namespace InfrastructureShared.Storage;

public class S3Config
{
    public string ServiceUrl { get; init; } = null!;
    public string AccessKey { get; init; } = null!;
    public string SecretKey { get; init; } = null!;
    public S3MainBucketConf MainBucket { get; init; } = null!;
}

public sealed record class S3MainBucketConf(string Name);