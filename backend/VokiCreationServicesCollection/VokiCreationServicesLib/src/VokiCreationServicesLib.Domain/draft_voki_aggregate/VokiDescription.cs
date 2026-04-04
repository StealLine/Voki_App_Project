using SharedKernel.exceptions;

namespace VokiCreationServicesLib.Domain.draft_voki_aggregate;

public sealed class VokiDescription : ValueObject
{
    private readonly string _value;

    private VokiDescription(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(value));
        _value = value ?? "";
    }

    public const int MaxLength = 2000;

    public override string ToString() => _value;

    public override IEnumerable<object> GetEqualityComponents() => [_value];

    public static ErrOr<VokiDescription> Create(string value)
        => CheckForErr(value).IsErr(out var err) ? err : new VokiDescription(value);

    public static ErrOrNothing CheckForErr(string description) {
        int len = description?.Length ?? 0;

        if (len > MaxLength) {
            return ErrFactory.ValueOutOfRange(
                $"Voki description must not exceed {MaxLength} characters",
                $"Provided description is {len} characters"
            );
        }

        return ErrOrNothing.Nothing;
    }
    public static VokiDescription Empty => new("");

}