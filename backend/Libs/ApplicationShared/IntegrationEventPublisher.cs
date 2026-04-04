using SharedKernel.integration_events;

namespace ApplicationShared;

public interface IIntegrationEventPublisher
{
    Task Publish<T>(T integrationEvent, CancellationToken ct) where T : class, IIntegrationEvent;
}


