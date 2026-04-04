using ApplicationShared;
using GeneralVokiTakingService.Domain.general_voki_aggregate.events;
using SharedKernel.domain;
using SharedKernel.integration_events.voki_content_saved;
using SharedKernel.integration_events.voki_taken;

namespace GeneralVokiTakingService.Application;

internal class DomainToIntegrationEventsHandler : IDomainToIntegrationEventsHandler,
    IDomainEventHandler<PublishedVokiCreatedEvent>,
    IDomainEventHandler<VokiGotNewVokiTakenRecordEvent>

// and all other domain events that need to be published as integration events
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public DomainToIntegrationEventsHandler(IIntegrationEventPublisher integrationEventPublisher) {
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(PublishedVokiCreatedEvent e, CancellationToken ct) =>
        await _integrationEventPublisher.Publish(new GeneralVokiContentSavedIntegrationEvent(
            e.VokiId, e.VokiContentKeys.Select(k => k.ToString()).ToArray()
        ), ct);

    public async Task Handle(VokiGotNewVokiTakenRecordEvent e, CancellationToken ct) =>
        await _integrationEventPublisher.Publish(new GeneralVokiTakenIntegrationEvent(
            e.VokiId, e.VokiTakerId, e.NewVokiTakingsCount
        ), ct);
}