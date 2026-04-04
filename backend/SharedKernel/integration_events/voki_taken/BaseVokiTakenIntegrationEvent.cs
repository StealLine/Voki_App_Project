using System.Text.Json.Serialization;
using SharedKernel.common.vokis;

namespace SharedKernel.integration_events.voki_taken;
// @formatter:off
[JsonDerivedType(typeof(GeneralVokiTakenIntegrationEvent), typeDiscriminator: nameof(GeneralVokiTakenIntegrationEvent))]
// @formatter:on
public abstract record class BaseVokiTakenIntegrationEvent(
    VokiId VokiId,
    AppUserId? VokiTakerId,
    VokiType VokiType,
    uint NewVokiTakingsCount
) : IIntegrationEvent;