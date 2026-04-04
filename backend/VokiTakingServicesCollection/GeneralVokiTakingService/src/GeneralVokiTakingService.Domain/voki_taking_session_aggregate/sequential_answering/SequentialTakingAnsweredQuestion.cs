namespace GeneralVokiTakingService.Domain.voki_taking_session_aggregate.sequential_answering;

public record SequentialTakingAnsweredQuestion(
    GeneralVokiQuestionId QuestionId,
    ImmutableHashSet<GeneralVokiAnswerId> ChosenAnswerIds,
    DateTime ClientShownAt,
    DateTime ClientSubmittedAt
);