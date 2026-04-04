namespace SharedKernel.integration_events;

public record class VokiRatingsCountChangedIntegrationEvent(
    VokiId VokiId,
    uint NewRatingsCount
) : IIntegrationEvent;