namespace CoreVokiCreationService.Domain.draft_voki_aggregate.events;

public record class CoAuthorInviteCanceledEvent(
    VokiId VokiId,
    AppUserId AppUserId
) : IDomainEvent;