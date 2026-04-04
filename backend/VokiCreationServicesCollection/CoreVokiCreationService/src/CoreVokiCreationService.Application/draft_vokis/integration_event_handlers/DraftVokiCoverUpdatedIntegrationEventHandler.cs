using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate;
using MassTransit;
using SharedKernel.integration_events.draft_vokis;
using VokimiStorageKeysLib.concrete_keys;

namespace CoreVokiCreationService.Application.draft_vokis.integration_event_handlers;

public class DraftVokiCoverUpdatedIntegrationEventHandler : IConsumer<DraftVokiCoverUpdatedIntegrationEvent>
{
    private readonly IDraftVokiRepository _draftVokiRepository;

    public DraftVokiCoverUpdatedIntegrationEventHandler(IDraftVokiRepository draftVokiRepository) {
        _draftVokiRepository = draftVokiRepository;
    }

    public async Task Consume(ConsumeContext<DraftVokiCoverUpdatedIntegrationEvent> context) {
        DraftVoki voki = (await _draftVokiRepository.GetByIdForUpdate(context.Message.VokiId, context.CancellationToken))!;
        var res = voki.UpdateCover(new VokiCoverKey(context.Message.NewVokiCover));
        UnexpectedBehaviourException.ThrowIfErr(res);
        await _draftVokiRepository.Update(voki, context.CancellationToken);
    }
}