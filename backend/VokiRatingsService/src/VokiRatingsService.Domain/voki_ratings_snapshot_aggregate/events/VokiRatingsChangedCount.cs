namespace VokiRatingsService.Domain.voki_ratings_snapshot_aggregate.events;
public record VokiRatingsChangedCount(
    VokiId VokiId,
    uint NewRatingsCount
) : IDomainEvent;