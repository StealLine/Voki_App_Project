
namespace SharedKernel.integration_events.draft_vokis;

public record class DraftVokiNameUpdatedIntegrationEvent(
    VokiId VokiId,
    string NewVokiName
) : IIntegrationEvent;