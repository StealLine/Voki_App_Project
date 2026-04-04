using SharedKernel.exceptions;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;

public sealed class VokiQuestionText : ValueObject
{
    private readonly string _value;

    public const int QuestionTextMinLength = 10;
    public const int QuestionTextMaxLength = 1000;

    private VokiQuestionText(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(value));
        _value = value;
    }

    public static ErrOr<VokiQuestionText> Create(string value) =>
        CheckForErr(value).IsErr(out var err) ? err : new VokiQuestionText(value);

    public static ErrOrNothing CheckForErr(string text) {
        if (string.IsNullOrWhiteSpace(text)) {
            return ErrFactory.NoValue.Common("Question text cannot be empty");
        }

        ErrOrNothing errs = ErrOrNothing.Nothing;

        if (text.Length < QuestionTextMinLength) {
            errs.AddNext(ErrFactory.ValueOutOfRange(
                $"Question text must be at least {QuestionTextMinLength} characters",
                $"Provided text is {text.Length} characters"
            ));
        }
        else if (text.Length > QuestionTextMaxLength) {
            errs.AddNext(ErrFactory.ValueOutOfRange(
                $"Question text must not exceed {QuestionTextMaxLength} characters",
                $"Provided text is {text.Length} characters"
            ));
        }

        if (char.IsWhiteSpace(text[0])) {
            errs.AddNext(ErrFactory.IncorrectFormat("Question text cannot start with a space"));
        }

        if (char.IsWhiteSpace(text[^1])) {
            errs.AddNext(ErrFactory.IncorrectFormat("Question text cannot end with a space"));
        }

        return errs;
    }

    public override string ToString() => _value;

    public override IEnumerable<object> GetEqualityComponents() => [_value];
}