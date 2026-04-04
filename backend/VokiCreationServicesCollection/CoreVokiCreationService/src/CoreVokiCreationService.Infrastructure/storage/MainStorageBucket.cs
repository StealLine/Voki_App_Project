using Amazon.S3;
using CoreVokiCreationService.Application.common;
using InfrastructureShared.Storage;
using Microsoft.Extensions.Logging;
using VokimiStorageKeysLib.concrete_keys;

namespace CoreVokiCreationService.Infrastructure.storage;

internal class MainStorageBucket : BaseMainS3Bucket, IMainStorageBucket
{
    public MainStorageBucket(
        IAmazonS3 s3Client,
        S3MainBucketConf s3MainBucketConf,
        ILogger<MainStorageBucket> logger
    ) : base(s3Client, s3MainBucketConf, logger) { }


    public Task<ErrOrNothing> CopyDefaultVokiCoverForNewVoki(
        VokiCoverKey destination, CancellationToken ct
    ) => CopyStandardToStandard(
        source: CommonStorageItemKey.DefaultVokiCover,
        destination: destination,
        ct
    );
}