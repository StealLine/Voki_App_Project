using SharedKernel.exceptions;

namespace GeneralVokiTakingService.Domain.general_voki_aggregate.questions;

public class QuestionAnswersCountLimit : ValueObject
{
    private const ushort
        MinPossibleAnswersLimit = 1;

    public bool IsMultipleChoice => MinAnswers != 1 || MaxAnswers != 1;
    public ushort MinAnswers { get; }
    public ushort MaxAnswers { get; }

    public QuestionAnswersCountLimit(ushort minAnswers, ushort maxAnswers) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckAnswersForErr(minAnswers, maxAnswers));
        MinAnswers = minAnswers;
        MaxAnswers = maxAnswers;
    }
    private static ErrOrNothing CheckAnswersForErr(ushort minAnswers, ushort maxAnswers) {
        if (minAnswers < MinPossibleAnswersLimit) {
            return ErrFactory.IncorrectFormat(
                "Minimum answers value is less than allowed",
                $"Provided value '{minAnswers}' is less than the minimum allowed of {MinPossibleAnswersLimit}");
        }

        if (minAnswers > maxAnswers) {
            return ErrFactory.Conflict(
                "Invalid range for answers count limit. Minimum answers value is greater than the maximum answers value",
                $"Minimum answers '{minAnswers}' cannot be greater than maximum answers '{maxAnswers}'");
        }
        return ErrOrNothing.Nothing;
    }

    public override IEnumerable<object> GetEqualityComponents() => [IsMultipleChoice, MinAnswers, MaxAnswers];
}