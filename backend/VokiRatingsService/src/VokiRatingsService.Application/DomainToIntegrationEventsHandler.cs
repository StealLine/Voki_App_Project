using ApplicationShared;
using SharedKernel.domain;
using SharedKernel.integration_events;
using VokiRatingsService.Domain.voki_ratings_snapshot_aggregate.events;

namespace VokiRatingsService.Application;

internal class DomainToIntegrationEventsHandler : IDomainToIntegrationEventsHandler,
    IDomainEventHandler<VokiRatingsChangedCount>
// and all other domain events that need to be published as integration events
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public DomainToIntegrationEventsHandler(IIntegrationEventPublisher integrationEventPublisher) {
        _integrationEventPublisher = integrationEventPublisher;
    }


    public async Task Handle(VokiRatingsChangedCount e, CancellationToken ct) =>
        await _integrationEventPublisher.Publish(new VokiRatingsCountChangedIntegrationEvent(
            e.VokiId, e.NewRatingsCount
        ), ct);
}