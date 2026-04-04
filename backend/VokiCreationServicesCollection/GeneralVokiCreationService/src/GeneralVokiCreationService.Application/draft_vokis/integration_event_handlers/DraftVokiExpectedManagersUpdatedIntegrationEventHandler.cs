using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel.common.vokis;
using SharedKernel.integration_events.draft_vokis;

namespace GeneralVokiCreationService.Application.draft_vokis.integration_event_handlers;

public class DraftVokiExpectedManagersUpdatedIntegrationEventHandler : IConsumer<DraftVokiExpectedManagersUpdatedIntegrationEvent>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly ILogger<DraftVokiExpectedManagersUpdatedIntegrationEventHandler> _logger;

    public DraftVokiExpectedManagersUpdatedIntegrationEventHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        ILogger<DraftVokiExpectedManagersUpdatedIntegrationEventHandler> logger
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DraftVokiExpectedManagersUpdatedIntegrationEvent> context) {
        var msg = context.Message;
        var eventName = nameof(DraftVokiExpectedManagersUpdatedIntegrationEvent);
        
        if (msg.VokiType != VokiType.General) {
            _logger.LogInformation(
                "Skipping {EventName} for Voki {VokiId}: unsupported Voki type {VokiType}", eventName, msg.VokiId, msg.VokiType
            );
            return;
        }
        
        DraftGeneralVoki? voki = await _draftGeneralVokisRepository.GetByIdForUpdate(msg.VokiId, context.CancellationToken);
        if (voki is null) {
            _logger.LogWarning(
                "Received {EventName} but draft voki {VokiId} was not found. Could not update expected managers",
                eventName, msg.VokiId
            );
            return;
        }
        
        voki.UpdateExpectedManagers(msg.ExpectedManagers.ToImmutableHashSet());
        await _draftGeneralVokisRepository.Update(voki, context.CancellationToken);
    }
}