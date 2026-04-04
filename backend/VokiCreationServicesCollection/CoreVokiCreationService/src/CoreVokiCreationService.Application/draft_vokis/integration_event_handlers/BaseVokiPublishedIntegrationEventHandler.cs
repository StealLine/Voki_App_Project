using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate;
using MassTransit;
using SharedKernel.integration_events.voki_publishing;

namespace CoreVokiCreationService.Application.draft_vokis.integration_event_handlers;

public class BaseVokiPublishedIntegrationEventHandler : IConsumer<BaseVokiPublishedIntegrationEvent>
{
    private readonly IDraftVokiRepository _draftVokiRepository;

    public BaseVokiPublishedIntegrationEventHandler(IDraftVokiRepository draftVokiRepository) {
        _draftVokiRepository = draftVokiRepository;
    }

    public async Task Consume(ConsumeContext<BaseVokiPublishedIntegrationEvent> context) {
        DraftVoki? voki = await _draftVokiRepository.GetByIdForUpdate(context.Message.VokiId, context.CancellationToken);
        if (voki is null) {
            return;
        }

        voki.MarkAsPublished();
        await _draftVokiRepository.Delete(voki, context.CancellationToken);
    }
}