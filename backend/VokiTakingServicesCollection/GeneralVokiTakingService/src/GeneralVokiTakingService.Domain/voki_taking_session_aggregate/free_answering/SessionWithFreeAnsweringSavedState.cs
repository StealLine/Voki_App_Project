namespace GeneralVokiTakingService.Domain.voki_taking_session_aggregate.free_answering;

public record class SessionWithFreeAnsweringSavedState(
    ImmutableDictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> QuestionsWithAnswers,
    DateTime SaveTime
)
{
    public static SessionWithFreeAnsweringSavedState CreateEmpty(
        IEnumerable<TakingSessionExpectedQuestion> questions,
        DateTime time
    ) => new(
        questions.ToImmutableDictionary(
            q => q.QuestionId,
            _ => ImmutableHashSet<GeneralVokiAnswerId>.Empty
        ),
        time
    );
}