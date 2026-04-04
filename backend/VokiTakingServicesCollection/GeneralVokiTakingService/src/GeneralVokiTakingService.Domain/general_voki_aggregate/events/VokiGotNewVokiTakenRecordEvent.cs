namespace GeneralVokiTakingService.Domain.general_voki_aggregate.events;

public record VokiGotNewVokiTakenRecordEvent(
    VokiId VokiId,
    AppUserId? VokiTakerId,
    uint NewVokiTakingsCount
) : IDomainEvent;