using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeneralVokiCreationService.Infrastructure.persistence.configurations.value_converters.questions;

public class QuestionAnswersCountLimitConverter : ValueConverter<QuestionAnswersCountLimit, string>
{
    public QuestionAnswersCountLimitConverter() : base(
        limit => ToString(limit),
        str => FromString(str)
    ) { }
    private static string ToString(QuestionAnswersCountLimit value) {
        if (!value.IsMultipleChoice) {
            return "SingleChoice";
        }

        return $"{value.MinAnswers}-{value.MaxAnswers}";
    }
    private static QuestionAnswersCountLimit FromString(string str) {
        if (str == "SingleChoice") {
            return QuestionAnswersCountLimit.SingleChoice();
        }

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

        var res = QuestionAnswersCountLimit.MultipleChoice(min, max);
        UnexpectedBehaviourException.ThrowIfErr(res);
        return res.AsSuccess();
    }

  
}