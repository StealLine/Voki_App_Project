using SharedKernel.exceptions;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;

public sealed class VokiResultName : ValueObject
{
    private readonly string _value;

    public const int MinLength = 2;
    public const int MaxLength = 120;

    private VokiResultName(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(value));
        _value = value;
    }

    public static ErrOr<VokiResultName> Create(string value) =>
        CheckForErr(value).IsErr(out var err) ? err : new VokiResultName(value);

    public static ErrOrNothing CheckForErr(string text) {
        if (string.IsNullOrWhiteSpace(text)) {
            return ErrFactory.NoValue.Common("Result name cannot be empty");
        }

        ErrOrNothing errs = ErrOrNothing.Nothing;

        if (text.Length < MinLength) {
            errs.AddNext(ErrFactory.ValueOutOfRange(
                $"Result name must be at least {MinLength} characters",
                $"Provided name is {text.Length} characters"
            ));
        }
        else if (text.Length > MaxLength) {
            errs.AddNext(ErrFactory.ValueOutOfRange(
                $"Result name must not exceed {MaxLength} characters",
                $"Provided name is {text.Length} characters"
            ));
        }

        if (char.IsWhiteSpace(text[0])) {
            errs.AddNext(ErrFactory.IncorrectFormat("Result name cannot start with a space"));
        }

        if (char.IsWhiteSpace(text[^1])) {
            errs.AddNext(ErrFactory.IncorrectFormat("Result name cannot end with a space"));
        }

        return errs;
    }

    public override string ToString() => _value;

    public override IEnumerable<object> GetEqualityComponents() => [_value];
}
