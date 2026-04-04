using SharedKernel.common.vokis;

namespace SharedKernel.integration_events.draft_vokis;

public record class DraftVokiExpectedManagersUpdatedIntegrationEvent(
    VokiId VokiId,
    VokiType VokiType,
    AppUserId[] ExpectedManagers
) : IIntegrationEvent;