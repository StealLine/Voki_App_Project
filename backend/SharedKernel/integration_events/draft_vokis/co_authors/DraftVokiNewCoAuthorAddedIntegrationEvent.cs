using SharedKernel.common.vokis;

namespace SharedKernel.integration_events.draft_vokis.co_authors;

public record DraftVokiNewCoAuthorAddedIntegrationEvent(
    VokiId VokiId,
    AppUserId AppUserId,
    VokiType VokiType,
    AppUserId[] UserIdsExpectedToBecomeManagers
) : IIntegrationEvent;