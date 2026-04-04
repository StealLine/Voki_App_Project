namespace CoreVokiCreationService.Domain.app_user_aggregate.events;

public record class CoAuthorInviteDeclinedEvent(
    VokiId VokiId,
    AppUserId AppUserId
) : IDomainEvent;