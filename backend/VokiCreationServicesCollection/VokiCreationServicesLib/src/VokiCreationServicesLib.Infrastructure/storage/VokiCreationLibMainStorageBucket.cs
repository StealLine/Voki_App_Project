using Amazon.S3;
using InfrastructureShared.Storage;
using Microsoft.Extensions.Logging;
using SharedKernel.errs;
using VokiCreationServicesLib.Application.common;
using VokimiStorageKeysLib.concrete_keys;
using VokimiStorageKeysLib.temp_keys;

namespace VokiCreationServicesLib.Infrastructure.storage;

public class VokiCreationLibLibMainStorageBucket : BaseMainS3Bucket, IVokiCreationLibMainStorageBucket
{
    public VokiCreationLibLibMainStorageBucket(
        IAmazonS3 s3Client,
        S3MainBucketConf s3MainBucketConf,
        ILogger<VokiCreationLibLibMainStorageBucket> logger
    ) : base(s3Client, s3MainBucketConf, logger) { }

    public Task<ErrOrNothing> CopyDefaultVokiCoverForVoki(
        VokiCoverKey destination, CancellationToken ct
    ) => CopyStandardToStandard(
        source: CommonStorageItemKey.DefaultVokiCover,
        destination: destination,
        ct: ct
    );

    public Task<ErrOrNothing> CopyVokiCoverFromTempToStandard(
        TempImageKey temp,
        VokiCoverKey destination,
        CancellationToken ct
    ) =>
        CopyTempToStandard(
            source: temp,
            destination: destination,
            ct: ct
        );

}