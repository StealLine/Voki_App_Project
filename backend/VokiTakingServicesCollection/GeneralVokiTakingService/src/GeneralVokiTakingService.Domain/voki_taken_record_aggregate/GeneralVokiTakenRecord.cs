using GeneralVokiTakingService.Domain.common.dtos;
using GeneralVokiTakingService.Domain.voki_taken_record_aggregate.events;
using VokiTakingServicesLib.Domain.common;
using VokiTakingServicesLib.Domain.voki_taken_record_aggregate;

namespace GeneralVokiTakingService.Domain.voki_taken_record_aggregate;

public sealed class GeneralVokiTakenRecord : BaseVokiTakenRecord
{
    private GeneralVokiTakenRecord() { }
    public override VokiType VokiType => VokiType.General;
    public GeneralVokiResultId ReceivedResultId { get; }
    public ImmutableArray<VokiTakenQuestionDetails> QuestionDetails { get; }
    public bool WasWithSequentialAnswering { get; }

    private GeneralVokiTakenRecord(
        VokiTakenRecordId id,
        VokiId takenVokiId,
        AppUserId? vokiTakerId,
        DateTime startTime,
        DateTime finishTime,
        GeneralVokiResultId receivedResultId,
        ImmutableArray<VokiTakenQuestionDetails> questionDetails,
        bool wasWithSequentialAnswering
    ) : base(id, takenVokiId, vokiTakerId, startTime, finishTime) {
        ReceivedResultId = receivedResultId;
        QuestionDetails = questionDetails;
        WasWithSequentialAnswering = wasWithSequentialAnswering;
    }

    public static GeneralVokiTakenRecord CreateNew(VokiTakingSessionFinishedDto dto) {
        GeneralVokiTakenRecord newRecord = new(
            VokiTakenRecordId.CreateNew(), dto.TakenVokiId, dto.VokiTakerId,
            dto.SessionStartTime, dto.SessionFinishTime,
            dto.ReceivedResultId, dto.QuestionDetails, dto.WasSessionWithForceSequentialOrder
        );
        newRecord.AddDomainEvent(new VokiTakenRecordCreatedEvent(
            newRecord.Id,
            newRecord.TakenVokiId,
            newRecord.VokiTakerId,
            newRecord.ReceivedResultId
        ));
        return newRecord;
    }
}