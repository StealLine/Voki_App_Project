using SharedKernel.exceptions;

namespace GeneralVokiTakingService.Domain.general_voki_aggregate.questions.answers;

public sealed class GeneralVokiAnswerText : ValueObject
{
    private readonly string _value;

    public const int AnswerTextMinLength = 5;
    public const int AnswerTextMaxLength = 1000;

    private GeneralVokiAnswerText(string value)
    {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(value));
        _value = value;
    }

    public static ErrOr<GeneralVokiAnswerText> Create(string value) =>
        CheckForErr(value).IsErr(out var err) ? err : new GeneralVokiAnswerText(value);

    public static ErrOrNothing CheckForErr(string text)
    {
        int len = string.IsNullOrWhiteSpace(text) ? 0 : text.Length;

        if (len < AnswerTextMinLength)
        {
            return ErrFactory.IncorrectFormat(
                "Answer text is too short",
                $"Answer must be at least {AnswerTextMinLength} characters long"
            );
        }

        if (len > AnswerTextMaxLength)
        {
            return ErrFactory.IncorrectFormat(
                "Answer text is too long",
                $"Answer must not exceed {AnswerTextMaxLength} characters long"
            );
        }

        return ErrOrNothing.Nothing;
    }

    public override string ToString() => _value;

    public override IEnumerable<object> GetEqualityComponents() => [_value];
}
