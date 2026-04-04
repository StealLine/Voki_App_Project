using ApplicationShared;
using AuthService.Domain.app_user_aggregate.events;
using SharedKernel.domain;
using SharedKernel.integration_events;

namespace AuthService.Application;

internal class DomainToIntegrationEventsHandler : IDomainToIntegrationEventsHandler,
    IDomainEventHandler<NewAppUserCreatedEvent>
// and all other domain events that need to be published as integration events
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public DomainToIntegrationEventsHandler(IIntegrationEventPublisher integrationEventPublisher) {
        _integrationEventPublisher = integrationEventPublisher;
    }


    public async Task Handle(NewAppUserCreatedEvent e, CancellationToken ct) =>
        await _integrationEventPublisher.Publish(new NewAppUserCreatedIntegrationEvent(
            e.CreatedUserId, e.UserUniqueName.ToString()
        ), ct);
}