namespace SharedKernel.integration_events.voki_content_saved;

public record GeneralVokiContentSavedIntegrationEvent(
    VokiId VokiId,
    string[] VokiContentKeys
) : IIntegrationEvent;