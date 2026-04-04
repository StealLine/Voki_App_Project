using SharedKernel.common.vokis;
using VokimiStorageKeysLib.concrete_keys;

namespace CoreVokiCreationService.Domain.draft_voki_aggregate.events;

public record NewDraftVokiInitializedEvent(
    VokiId VokiId,
    VokiType Type,
    VokiName Name,
    VokiCoverKey Cover,    
    AppUserId PrimaryAuthorId,
    DateTime CreationDate
) : IDomainEvent;