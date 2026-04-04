namespace GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

public record TakingSessionExpectedQuestion(
    GeneralVokiQuestionId QuestionId,
    QuestionOrderInVokiTakingSession OrderInVokiTaking,
    ushort MinAnswersCount,
    ushort MaxAnswersCount,
    ImmutableDictionary<GeneralVokiAnswerId, ushort> AnswersIdToOrderInQuestion
);