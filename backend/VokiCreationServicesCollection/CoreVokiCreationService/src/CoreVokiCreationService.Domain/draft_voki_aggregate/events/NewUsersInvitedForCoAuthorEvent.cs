namespace CoreVokiCreationService.Domain.draft_voki_aggregate.events;

public record class NewUsersInvitedForCoAuthorEvent(
    ImmutableHashSet<AppUserId> NewInvitedUserId,
    VokiId VokiId
) : IDomainEvent;