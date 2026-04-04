using SharedKernel.exceptions;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;

public class QuestionAnswersCountLimit : ValueObject
{
    private const ushort
        MinPossibleAnswersLimit = 1,
        MaxPossibleAnswersLimit = VokiQuestion.MaxAnswersCount;

    public bool IsMultipleChoice => MinAnswers != 1 || MaxAnswers != 1;
    public ushort MinAnswers { get; }
    public ushort MaxAnswers { get; }

    private QuestionAnswersCountLimit(ushort minAnswers, ushort maxAnswers) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckAnswersForErr(minAnswers, maxAnswers));
        MinAnswers = minAnswers;
        MaxAnswers = maxAnswers;
    }

    public static QuestionAnswersCountLimit SingleChoice() => new(1, 1);

    public static ErrOr<QuestionAnswersCountLimit> MultipleChoice(ushort minAnswers, ushort maxAnswers) =>
        CheckAnswersForErr(maxAnswers, maxAnswers).IsErr(out var err)
            ? err
            : new QuestionAnswersCountLimit(minAnswers, maxAnswers);

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

        if (maxAnswers > MaxPossibleAnswersLimit) {
            return ErrFactory.IncorrectFormat(
                "Maximum answers value exceeds allowed limit",
                $"Provided maximum '{maxAnswers}' exceeds the allowed limit of {MaxPossibleAnswersLimit}");
        }

        return ErrOrNothing.Nothing;
    }

    public override IEnumerable<object> GetEqualityComponents() => [IsMultipleChoice, MinAnswers, MaxAnswers];
}