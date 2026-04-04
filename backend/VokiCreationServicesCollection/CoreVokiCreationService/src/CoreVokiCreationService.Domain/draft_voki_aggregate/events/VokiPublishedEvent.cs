namespace CoreVokiCreationService.Domain.draft_voki_aggregate.events;

public sealed record class VokiPublishedEvent(
    VokiId VokiId,
    AppUserId PrimaryAuthorId,
    ImmutableHashSet<AppUserId> CoAuthorsIds
) : IDomainEvent;