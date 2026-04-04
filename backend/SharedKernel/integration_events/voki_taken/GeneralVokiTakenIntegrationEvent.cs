using SharedKernel.common.vokis;

namespace SharedKernel.integration_events.voki_taken;

public record GeneralVokiTakenIntegrationEvent(
    VokiId VokiId,
    AppUserId? VokiTakerId,
    uint NewVokiTakingsCount
) : BaseVokiTakenIntegrationEvent(VokiId, VokiTakerId, VokiType.General, NewVokiTakingsCount);