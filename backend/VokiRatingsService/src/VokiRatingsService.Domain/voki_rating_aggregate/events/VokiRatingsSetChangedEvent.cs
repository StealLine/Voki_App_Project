
namespace VokiRatingsService.Domain.voki_rating_aggregate.events;

public record VokiRatingsSetChangedEvent(VokiId VokiId) : IDomainEvent;