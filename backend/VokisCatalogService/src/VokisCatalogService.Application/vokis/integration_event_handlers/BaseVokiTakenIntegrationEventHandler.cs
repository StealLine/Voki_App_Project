using MassTransit;
using SharedKernel;
using SharedKernel.integration_events.voki_taken;
using VokisCatalogService.Application.common.repositories;
using VokisCatalogService.Domain.app_user_aggregate;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Application.vokis.integration_event_handlers;

public class BaseVokiTakenIntegrationEventHandler : IConsumer<BaseVokiTakenIntegrationEvent>
{
    private readonly IVokisRepository _vokisRepository;
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public BaseVokiTakenIntegrationEventHandler(
        IVokisRepository vokisRepository,
        IAppUsersRepository appUsersRepository,
        IDateTimeProvider dateTimeProvider
    ) {
        _vokisRepository = vokisRepository;
        _appUsersRepository = appUsersRepository;
        _dateTimeProvider = dateTimeProvider;
    }


    public async Task Consume(ConsumeContext<BaseVokiTakenIntegrationEvent> context) {
        Voki? voki = await _vokisRepository.GetByIdForUpdate(context.Message.VokiId, context.CancellationToken);
        if (voki is null) {
            return;
        }

        voki.UpdateVokiTakingsCount(context.Message.NewVokiTakingsCount);
        await _vokisRepository.Update(voki, context.CancellationToken);

        if (context.Message.VokiTakerId is null) {
            return;
        }

        AppUser? vokiTaker = await _appUsersRepository.GetUserWithTakenVokisForUpdate(
            context.Message.VokiTakerId,
            context.CancellationToken
        );
        if (vokiTaker is null) {
            return;
        }

        vokiTaker.TakenVokis.Add(context.Message.VokiId, _dateTimeProvider.UtcNow);
        await _appUsersRepository.Update(vokiTaker, context.CancellationToken);
    }
}