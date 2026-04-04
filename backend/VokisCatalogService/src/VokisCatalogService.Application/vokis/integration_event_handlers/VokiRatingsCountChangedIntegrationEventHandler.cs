using MassTransit;
using SharedKernel.integration_events;
using VokisCatalogService.Application.common.repositories;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Application.vokis.integration_event_handlers;

public class VokiRatingsCountChangedIntegrationEventHandler : IConsumer<VokiRatingsCountChangedIntegrationEvent>
{
    private readonly IVokisRepository _vokisRepository;

    public VokiRatingsCountChangedIntegrationEventHandler(IVokisRepository vokisRepository) {
        _vokisRepository = vokisRepository;
    }

    public async Task Consume(ConsumeContext<VokiRatingsCountChangedIntegrationEvent> context) {
        Voki? voki = await _vokisRepository.GetByIdForUpdate(context.Message.VokiId, context.CancellationToken);
        if (voki is null) {
            return;
        }

        voki.UpdateRatingsCount(context.Message.NewRatingsCount);
        await _vokisRepository.Update(voki, context.CancellationToken);
    }
}