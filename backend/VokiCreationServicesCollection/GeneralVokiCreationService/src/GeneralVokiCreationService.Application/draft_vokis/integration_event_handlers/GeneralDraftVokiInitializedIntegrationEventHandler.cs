using MassTransit;
using SharedKernel.common.vokis;
using SharedKernel.integration_events.draft_vokis.new_voki_initialized;
using VokimiStorageKeysLib.concrete_keys;

namespace GeneralVokiCreationService.Application.draft_vokis.integration_event_handlers;

public class GeneralDraftVokiInitializedIntegrationEventHandler : IConsumer<GeneralDraftVokiInitializedIntegrationEvent>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;

    public GeneralDraftVokiInitializedIntegrationEventHandler(IDraftGeneralVokisRepository draftGeneralVokisRepository) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
    }

    public async Task Consume(ConsumeContext<GeneralDraftVokiInitializedIntegrationEvent> context) {
        DraftGeneralVoki newGeneralVoki = DraftGeneralVoki.Create(
            context.Message.VokiId,
            context.Message.PrimaryAuthorId,
            new VokiName(  context.Message.VokiName),
            new VokiCoverKey(context.Message.Cover),
            context.Message.CreationDate
        );
        await _draftGeneralVokisRepository.Add(newGeneralVoki, context.CancellationToken);
    }
}