namespace VokisCatalogService.Domain.voki_aggregate.events;

public record class PublishedVokiCreatedEvent(
    VokiId VokiId,
    AppUserId PrimaryAuthorId,
    ImmutableHashSet<AppUserId> CoAuthorIds,
    ImmutableHashSet<VokiTagId> Tags
) : IDomainEvent;