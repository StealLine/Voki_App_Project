using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel.common.vokis;
using SharedKernel.integration_events.draft_vokis.co_authors;

namespace GeneralVokiCreationService.Application.draft_vokis.integration_event_handlers;

public class DraftVokiNewCoAuthorAddedIntegrationEventHandler : IConsumer<DraftVokiNewCoAuthorAddedIntegrationEvent>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly ILogger<DraftVokiNewCoAuthorAddedIntegrationEventHandler> _logger;

    public DraftVokiNewCoAuthorAddedIntegrationEventHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        ILogger<DraftVokiNewCoAuthorAddedIntegrationEventHandler> logger
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DraftVokiNewCoAuthorAddedIntegrationEvent> context) {
        var msg = context.Message;
        var eventName = nameof(DraftVokiNewCoAuthorAddedIntegrationEvent);

        if (msg.VokiType != VokiType.General) {
            _logger.LogInformation(
                "Skipping {EventName} for Voki {VokiId}: unsupported Voki type {VokiType}",
                eventName, msg.VokiId, msg.VokiType
            );
            return;
        }

        DraftGeneralVoki? voki = await _draftGeneralVokisRepository.GetByIdForUpdate(msg.VokiId, context.CancellationToken);
        if (voki is null) {
            _logger.LogWarning(
                "Received {EventName} but draft voki {VokiId} was not found. Could not add co-author {AppUserId}",
                eventName, msg.VokiId, msg.AppUserId
            );
            return;
        }

        ErrOrNothing result = voki.AddCoAuthor(msg.AppUserId, msg.UserIdsExpectedToBecomeManagers.ToImmutableHashSet());

        if (result.IsErr(out var err)) {
            _logger.LogError(
                "Failed to handle {EventName} for Voki {VokiId}. Could not add co-author {AppUserId}. Err: {Err}",
                eventName, msg.VokiId, msg.AppUserId, err.ToString()
            );

            UnexpectedBehaviourException.ThrowErr(err);
        }

        await _draftGeneralVokisRepository.Update(voki, context.CancellationToken);

        _logger.LogInformation(
            "Processed {EventName}: co-author {AppUserId} added to Voki {VokiId}",
            eventName, msg.AppUserId, msg.VokiId
        );
    }
}