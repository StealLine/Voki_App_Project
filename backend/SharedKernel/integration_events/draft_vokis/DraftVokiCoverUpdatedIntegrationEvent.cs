namespace SharedKernel.integration_events.draft_vokis;

public record class DraftVokiCoverUpdatedIntegrationEvent(
    VokiId VokiId,
    string NewVokiCover
) : IIntegrationEvent;