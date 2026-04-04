using Amazon.S3;
using GeneralVokiCreationService.Application.common;
using InfrastructureShared.Storage;
using Microsoft.Extensions.Logging;
using VokimiStorageKeysLib.concrete_keys.general_voki;
using VokimiStorageKeysLib.temp_keys;

namespace GeneralVokiCreationService.Infrastructure.storage;

internal class MainStorageBucket : BaseMainS3Bucket, IMainStorageBucket
{
    public MainStorageBucket(
        IAmazonS3 s3Client,
        S3MainBucketConf s3MainBucketConf,
        ILogger<MainStorageBucket> logger
    ) : base(s3Client, s3MainBucketConf, logger) { }

    public Task<ErrOrNothing> CopyVokiQuestionImagesFromTempToStandard(
        Dictionary<TempImageKey, GeneralVokiQuestionImageKey> tempToDestination, CancellationToken ct
    ) =>
        CopyMultipleTempToStandard(sourcesToDestinations: tempToDestination, ct);

    public Task<ErrOrNothing> CopyVokiAnswerImageKeysFromTempToStandard(
        Dictionary<TempImageKey, GeneralVokiAnswerImageKey> sourcesToDestinations, CancellationToken ct
    ) => 
        base.CopyMultipleTempToStandard(sourcesToDestinations: sourcesToDestinations, ct: ct);

    public Task<ErrOrNothing> CopyVokiAnswerAudioKeysFromTempToStandard(
        Dictionary<TempAudioKey, GeneralVokiAnswerAudioKey> sourcesToDestinations, CancellationToken ct
    ) =>
        base.CopyMultipleTempToStandard(sourcesToDestinations: sourcesToDestinations, ct: ct);

    public Task<ErrOrNothing> CopyVokiResultImageFromTempToStandard(
        TempImageKey temp,
        GeneralVokiResultImageKey destination,
        CancellationToken ct
    ) =>
        CopyTempToStandard(
            source: temp,
            destination: destination,
            ct
        );


    public Task<ErrOrNothing> CopyVokiAnswerImageFromTempToStandard(
        TempImageKey temp,
        GeneralVokiAnswerImageKey destination,
        CancellationToken ct
    ) =>
        CopyTempToStandard(
            source: temp,
            destination: destination,
            ct
        );

    public Task<ErrOrNothing> CopyVokiAnswerAudioFromTempToStandard(
        TempAudioKey temp,
        GeneralVokiAnswerAudioKey destination,
        CancellationToken ct
    ) =>
        CopyTempToStandard(
            source: temp,
            destination: destination,
            ct
        );
}