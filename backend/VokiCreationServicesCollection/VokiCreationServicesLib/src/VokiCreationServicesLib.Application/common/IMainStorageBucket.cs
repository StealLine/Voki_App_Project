using SharedKernel.errs;
using VokimiStorageKeysLib.concrete_keys;
using VokimiStorageKeysLib.temp_keys;

namespace VokiCreationServicesLib.Application.common;

public interface IVokiCreationLibMainStorageBucket
{
    Task<ErrOrNothing> CopyDefaultVokiCoverForVoki(VokiCoverKey defaultVokiCover, CancellationToken ct);
    Task<ErrOrNothing> CopyVokiCoverFromTempToStandard(TempImageKey temp, VokiCoverKey destination, CancellationToken ct);

}