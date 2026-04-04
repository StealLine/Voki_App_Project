using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.value_converters.voki_taking_sessions;

internal class TakingSessionExpectedQuestionsArrayConverter :
    ValueConverter<ImmutableArray<TakingSessionExpectedQuestion>, string[]>
{
    public TakingSessionExpectedQuestionsArrayConverter() : base(
        questions => ToStringArray(questions),
        strings => FromStringArray(strings)
    ) { }

    private const char Sep = '|';
    private const int PartsCount = 5;

    private static string[] ToStringArray(ImmutableArray<TakingSessionExpectedQuestion> questions) =>
        questions.Select(QuestionToString).ToArray();

    private static string QuestionToString(TakingSessionExpectedQuestion q) =>
        $"{q.QuestionId}{Sep}{q.OrderInVokiTaking}{Sep}{q.MinAnswersCount}{Sep}{q.MaxAnswersCount}{Sep}{AnswersToString(q.AnswersIdToOrderInQuestion)}";

    private static string AnswersToString(ImmutableDictionary<GeneralVokiAnswerId, ushort> answers) =>
        string.Join(',', answers.Select(kv => $"{kv.Key}:{kv.Value}"));

    private static ImmutableArray<TakingSessionExpectedQuestion> FromStringArray(string[] strs) {
        List<TakingSessionExpectedQuestion> questions = new(strs.Length);
        foreach (string str in strs) {
            string[] parts = str.Split(Sep);
            if (parts.Length != PartsCount) {
                UnexpectedBehaviourException.ThrowErr(ErrFactory.IncorrectFormat(
                    $"Wrong number of parts {parts.Length} (expected {PartsCount}) in string: {str}"
                ));
            }

            GeneralVokiQuestionId questionId = new(new(parts[0]));
            var orderInVokiTaking = QuestionOrderInVokiTakingSession.Create(ushort.Parse(parts[1])).AsSuccess();

            ushort minAnswersCount = ushort.Parse(parts[2]);
            ushort maxAnswersCount = ushort.Parse(parts[3]);
            ImmutableDictionary<GeneralVokiAnswerId, ushort> answersIdToOrder = parts[4]
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(pair => {
                    var p = pair.Split(':');
                    return new { Id = new GeneralVokiAnswerId(new(p[0])), Order = ushort.Parse(p[1]) };
                })
                .ToImmutableDictionary(x => x.Id, x => x.Order);

            questions.Add(new TakingSessionExpectedQuestion(
                questionId, orderInVokiTaking, minAnswersCount, maxAnswersCount, answersIdToOrder
            ));
        }

        return questions.ToImmutableArray();
    }
}

internal class TakingSessionExpectedQuestionsArrayComparer : ValueComparer<ImmutableArray<TakingSessionExpectedQuestion>>
{
    public TakingSessionExpectedQuestionsArrayComparer() : base(
        (t1, t2) => t1.SequenceEqual(t2!, EqualityComparer<TakingSessionExpectedQuestion>.Default),
        t => t.Select(x => x!.GetHashCode()).Aggregate((x, y) => x ^ y),
        t => t.ToImmutableArray()
    ) { }
}