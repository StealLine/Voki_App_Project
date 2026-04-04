using ApplicationShared;
using MassTransit;
using SharedKernel.integration_events;

namespace GeneralVokiTakingService.Infrastructure;

public class IntegrationEventPublisher : IIntegrationEventPublisher
{
    private readonly IPublishEndpoint _publish;

    public IntegrationEventPublisher(IPublishEndpoint publish) {
        _publish = publish;
    }

    public Task Publish<T>(T integrationEvent, CancellationToken ct) where T : class, IIntegrationEvent =>
        _publish.Publish(integrationEvent, ct);
}