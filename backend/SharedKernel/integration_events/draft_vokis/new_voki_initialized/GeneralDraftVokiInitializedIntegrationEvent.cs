namespace SharedKernel.integration_events.draft_vokis.new_voki_initialized;

public record class GeneralDraftVokiInitializedIntegrationEvent(
    VokiId VokiId,
    AppUserId PrimaryAuthorId,
    string VokiName,
    string Cover,
    DateTime CreationDate
) : IIntegrationEvent;