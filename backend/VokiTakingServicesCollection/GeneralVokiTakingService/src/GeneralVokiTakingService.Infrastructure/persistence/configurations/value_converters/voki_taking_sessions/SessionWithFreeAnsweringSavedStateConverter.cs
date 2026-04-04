using GeneralVokiTakingService.Domain.voki_taking_session_aggregate.free_answering;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.value_converters.voki_taking_sessions;

public class SessionWithFreeAnsweringSavedStateConverter : ValueConverter<SessionWithFreeAnsweringSavedState, string>
{
    private const char MainSeparator = '|';
    private const char QuestionSeparator = ';';
    private const char IdSeparator = ':';
    private const char AnswerSeparator = ',';

    public SessionWithFreeAnsweringSavedStateConverter() : base(
        v => ToString(v),
        s => FromString(s)
    ) { }

    private static string ToString(SessionWithFreeAnsweringSavedState state) {
        string questionsStr = string.Join(
            QuestionSeparator,
            state.QuestionsWithAnswers.Select(kvp => $"{kvp.Key}{IdSeparator}{string.Join(AnswerSeparator, kvp.Value)}")
        );

        return $"{state.SaveTime:O}{MainSeparator}{questionsStr}";
    }

    private static SessionWithFreeAnsweringSavedState FromString(string s) {
        if (string.IsNullOrWhiteSpace(s)) {
            return new SessionWithFreeAnsweringSavedState(
                ImmutableDictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>>.Empty, DateTime.MinValue
            );
        }

        string[] parts = s.Split(MainSeparator);
        DateTime saveTime =DateTimeOffset.Parse(parts[0]).UtcDateTime;

        if (parts.Length <= 1 || string.IsNullOrWhiteSpace(parts[1])) {
            return new SessionWithFreeAnsweringSavedState(
                ImmutableDictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>>.Empty, saveTime
            );
        }

        ImmutableDictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> dict = parts[1]
            .Split(QuestionSeparator, StringSplitOptions.RemoveEmptyEntries)
            .Select(qStr => {
                var qParts = qStr.Split(IdSeparator);
                var qId = new GeneralVokiQuestionId(new Guid(qParts[0]));
                var aIds = qParts.Length > 1 && !string.IsNullOrWhiteSpace(qParts[1])
                    ? qParts[1].Split(AnswerSeparator, StringSplitOptions.RemoveEmptyEntries)
                        .Select(aId => new GeneralVokiAnswerId(new Guid(aId))).ToImmutableHashSet()
                    : ImmutableHashSet<GeneralVokiAnswerId>.Empty;
                return new { qId, aIds };
            })
            .ToImmutableDictionary(x => x.qId, x => x.aIds);

        return new SessionWithFreeAnsweringSavedState(dict, saveTime);
    }
}