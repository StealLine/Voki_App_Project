using VokiCreationServicesLib.Application.common;
using VokimiStorageKeysLib.concrete_keys;
using VokimiStorageKeysLib.concrete_keys.general_voki;
using VokimiStorageKeysLib.temp_keys;

namespace GeneralVokiCreationService.Application.common;

public interface IMainStorageBucket 
{


    Task<ErrOrNothing> CopyVokiResultImageFromTempToStandard(
        TempImageKey temp,
        GeneralVokiResultImageKey destination,
        CancellationToken ct
    );

    Task<ErrOrNothing> CopyVokiQuestionImagesFromTempToStandard(
        Dictionary<TempImageKey, GeneralVokiQuestionImageKey> tempToDestination,
        CancellationToken ct
    );

    Task<ErrOrNothing> CopyVokiAnswerImageKeysFromTempToStandard(
        Dictionary<TempImageKey,GeneralVokiAnswerImageKey>  sourcesToDestinations,
        CancellationToken ct
    );

    Task<ErrOrNothing> CopyVokiAnswerAudioKeysFromTempToStandard(
        Dictionary<TempAudioKey,GeneralVokiAnswerAudioKey>  sourcesToDestinations,
        CancellationToken ct
    );
}