using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

namespace GeneralVokiTakingService.Domain.voki_taken_record_aggregate;

public record VokiTakenQuestionDetails(
    GeneralVokiQuestionId QuestionId,
    ImmutableHashSet<GeneralVokiAnswerId> ChosenAnswerIds,
    QuestionOrderInVokiTakingSession OrderInVokiTaking
);