using SharedKernel.common.vokis;

namespace CoreVokiCreationService.Domain.draft_voki_aggregate.events;

public record class DraftVokiExpectedManagersUpdatedEvent(
    VokiId VokiId,
    VokiType VokiType,
    ImmutableHashSet<AppUserId> ExpectedManagers
) : IDomainEvent;