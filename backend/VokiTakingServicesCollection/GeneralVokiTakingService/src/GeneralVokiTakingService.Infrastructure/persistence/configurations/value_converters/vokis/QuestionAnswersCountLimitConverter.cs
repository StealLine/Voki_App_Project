using GeneralVokiTakingService.Domain.general_voki_aggregate.questions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.value_converters.vokis;

public class QuestionAnswersCountLimitConverter : ValueConverter<QuestionAnswersCountLimit, string>
{
    public QuestionAnswersCountLimitConverter() : base(
        limit => ToString(limit),
        str => FromString(str)
    ) { }

    private static string ToString(QuestionAnswersCountLimit value) => 
        $"{value.MinAnswers}-{value.MaxAnswers}";

    private static QuestionAnswersCountLimit FromString(string str) {
        string[] parts = str.Split('-');
        if (parts.Length != 2) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.IncorrectFormat(
                "Invalid format for Answer Count Limit", "Expected format 'min-max'"
            ));
        }

        if (!ushort.TryParse(parts[0], out var min)) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.IncorrectFormat(
                $"Value '{parts[0]}' cannot be parsed as a valid number for the minimum answers."
            ));
        }

        if (!ushort.TryParse(parts[1], out var max)) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.IncorrectFormat(
                $"Value '{parts[1]}' cannot be parsed as a valid number for the maximum answers"
            ));
        }

        return new QuestionAnswersCountLimit(min, max);
    }
}