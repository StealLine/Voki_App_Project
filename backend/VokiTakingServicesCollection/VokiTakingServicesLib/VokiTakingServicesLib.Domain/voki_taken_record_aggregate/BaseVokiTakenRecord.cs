using SharedKernel.common.vokis;
using SharedKernel.domain;
using SharedKernel.domain.ids;
using VokiTakingServicesLib.Domain.common;

namespace VokiTakingServicesLib.Domain.voki_taken_record_aggregate;

public abstract class BaseVokiTakenRecord : AggregateRoot<VokiTakenRecordId>
{
    protected BaseVokiTakenRecord() { }
    public VokiId TakenVokiId { get; }
    public AppUserId? VokiTakerId { get; }
    public abstract VokiType VokiType { get; }
    public DateTime StartTime { get; }
    public DateTime FinishTime { get; }

    protected BaseVokiTakenRecord(
        VokiTakenRecordId id,
        VokiId takenVokiId,
        AppUserId? vokiTakerId,
        DateTime startTime,
        DateTime finishTime
    ) {
        Id = id;
        TakenVokiId = takenVokiId;
        VokiTakerId = vokiTakerId;
        StartTime = startTime;
        FinishTime = finishTime;
        VokiTakerId = vokiTakerId;
    }
}