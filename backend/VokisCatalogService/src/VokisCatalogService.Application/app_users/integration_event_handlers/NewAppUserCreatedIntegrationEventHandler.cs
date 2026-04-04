using MassTransit;
using SharedKernel.integration_events;
using VokisCatalogService.Application.common.repositories;
using VokisCatalogService.Domain.app_user_aggregate;

namespace VokisCatalogService.Application.app_users.integration_event_handlers;

public class NewAppUserCreatedIntegrationEventHandler : IConsumer<NewAppUserCreatedIntegrationEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public NewAppUserCreatedIntegrationEventHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }

    public async Task Consume(ConsumeContext<NewAppUserCreatedIntegrationEvent> context) {
        AppUser user = new AppUser(context.Message.CreatedUserId);
        await _appUsersRepository.Add(user, context.CancellationToken);
    }
}