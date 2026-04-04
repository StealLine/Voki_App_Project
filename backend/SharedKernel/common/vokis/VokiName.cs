namespace SharedKernel.common.vokis;

public sealed class VokiName : ValueObject
{
    private readonly string _value;

    public VokiName(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(value));
        _value = value;
    }

    public const int MinLength = 3;
    public const int MaxLength = 500;

    public override string ToString() => _value;

    public override IEnumerable<object> GetEqualityComponents() => [_value];

    public static ErrOr<VokiName> Create(string value) =>
        CheckForErr(value).IsErr(out var err) ? err : new VokiName(value);

    public static ErrOrNothing CheckForErr(string name) {
        if (string.IsNullOrWhiteSpace(name)) {
            return ErrFactory.NoValue.Common("Voki name cannot be empty");
        }

        ErrOrNothing errs = ErrOrNothing.Nothing;
        if (name.Length < MinLength) {
            errs.AddNext(ErrFactory.ValueOutOfRange(
                $"Voki name must be at least {MinLength} characters", $"Provided name is {name.Length} characters"
            ));
        }
        else if (name.Length > MaxLength) {
            errs.AddNext(ErrFactory.ValueOutOfRange(
                $"Voki name must not exceed {MaxLength} characters", $"Provided name is {name.Length} characters"
            ));
        }

        if (char.IsWhiteSpace(name[0])) {
            errs.AddNext(ErrFactory.IncorrectFormat("Voki name cannot start with a space"));
        }

        if (char.IsWhiteSpace(name[^1])) {
            errs.AddNext(ErrFactory.IncorrectFormat("Voki name cannot end with a space"));
        }

        return errs;
    }
}