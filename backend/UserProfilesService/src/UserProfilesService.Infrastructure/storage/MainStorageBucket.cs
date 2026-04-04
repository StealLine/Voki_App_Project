using Amazon.S3;
using InfrastructureShared.Storage;
using Microsoft.Extensions.Logging;
using UserProfilesService.Application;
using UserProfilesService.Application.common;
using VokimiStorageKeysLib.concrete_keys;
using VokimiStorageKeysLib.concrete_keys.profile_pics;
using VokimiStorageKeysLib.temp_keys;

namespace UserProfilesService.Infrastructure.storage;

internal class MainStorageBucket : BaseMainS3Bucket, IMainStorageBucket
{
    public MainStorageBucket(
        IAmazonS3 s3Client,
        S3MainBucketConf s3MainBucketConf,
        ILogger<MainStorageBucket> logger
    ) : base(s3Client, s3MainBucketConf, logger) { }


    public async Task<ErrOr<UserProfilePicKey>> CopyUserProfilePicFromPresets(
        PresetProfilePicKey presetKey,
        AppUserId userId,
        CancellationToken ct
    ) {
        UserProfilePicKey newKey = UserProfilePicKey.CreateNewForUser(userId, presetKey.ImageExtension).AsSuccess();

        ErrOrNothing res = await CopyStandardToStandard(
            source: presetKey,
            destination: newKey,
            ct
        );
        if (res.IsErr(out var err)) {
            return err;
        }

        return newKey;
    }

    public async Task<ErrOr<UserProfilePicKey>> CopyUserProfilePicFromTemp(
        TempImageKey temp,
        AppUserId userId,
        CancellationToken ct
    ) {
        UserProfilePicKey newKey = UserProfilePicKey.CreateNewForUser(userId, temp.Extension).AsSuccess();

        ErrOrNothing res = await CopyTempToStandard(
            source: temp,
            destination: newKey,
            ct
        );
        if (res.IsErr(out var err)) {
            return err;
        }

        return newKey;
    }
}