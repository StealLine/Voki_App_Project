using VokimiStorageKeysLib.concrete_keys;

namespace CoreVokiCreationService.Application.common;

public interface IMainStorageBucket
{
    public Task<ErrOrNothing> CopyDefaultVokiCoverForNewVoki(VokiCoverKey destination, CancellationToken ct);
}