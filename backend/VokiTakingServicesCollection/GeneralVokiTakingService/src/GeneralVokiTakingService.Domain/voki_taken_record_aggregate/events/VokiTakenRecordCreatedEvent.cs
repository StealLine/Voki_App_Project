using VokiTakingServicesLib.Domain.common;

namespace GeneralVokiTakingService.Domain.voki_taken_record_aggregate.events;

public record class VokiTakenRecordCreatedEvent(
    VokiTakenRecordId VokiTakenRecordId,
    VokiId VokiId,
    AppUserId? VokiTakerId,
    GeneralVokiResultId ReceivedResultId
) : IDomainEvent;