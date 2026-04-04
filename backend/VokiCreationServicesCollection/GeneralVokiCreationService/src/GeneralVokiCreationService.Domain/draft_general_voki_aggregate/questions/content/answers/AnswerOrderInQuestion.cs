using SharedKernel.exceptions;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers;

public sealed class AnswerOrderInQuestion : ValueObject
{
    public ushort Value { get; }

    private AnswerOrderInQuestion(ushort value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(value));
        Value = value;
    }

    public static ErrOr<AnswerOrderInQuestion> Create(ushort value) =>
        CheckForErr(value).IsErr(out var err) ? err : new AnswerOrderInQuestion(value);

    private static ErrOrNothing CheckForErr(ushort val) => val switch {
        < 1 => ErrFactory.ValueOutOfRange(
            "Answer order must start with 1"
        ),
        > VokiQuestion.MaxAnswersCount => ErrFactory.IncorrectFormat(
            $"Question can have at most {VokiQuestion.MaxAnswersCount} answers, so the order value cannot exceed {VokiQuestion.MaxAnswersCount}"
        ),
        _ => ErrOrNothing.Nothing
    };

    public override IEnumerable<object> GetEqualityComponents() => [Value];
    public override string ToString() => Value.ToString();
}