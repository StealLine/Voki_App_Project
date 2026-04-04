using SharedKernel.common.vokis;

namespace CoreVokiCreationService.Domain.draft_voki_aggregate.events;

public record class VokiCoAuthorRemovedEvent(
    VokiId VokiId,
    AppUserId AppUserId,
    VokiType VokiType,
    ImmutableHashSet<AppUserId> UserIdsExpectedToBecomeManagers
) : IDomainEvent;