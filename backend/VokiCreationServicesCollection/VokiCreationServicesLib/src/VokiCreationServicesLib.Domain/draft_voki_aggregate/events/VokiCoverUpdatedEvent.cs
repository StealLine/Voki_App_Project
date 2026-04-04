using VokimiStorageKeysLib.concrete_keys;

namespace VokiCreationServicesLib.Domain.draft_voki_aggregate.events;

public record class VokiCoverUpdatedEvent(
    VokiId VokiId,
    VokiCoverKey OldCover,
    VokiCoverKey NewCover
) : IDomainEvent;