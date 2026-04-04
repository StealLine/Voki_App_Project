using SharedKernel.exceptions;

namespace VokiRatingsService.Domain.common;

public class RatingValue : ValueObject
{
    private RatingValue(ushort value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckValueForErr(value));
        Value = value;
    }

    public ushort Value { get; }
    public override IEnumerable<object> GetEqualityComponents() => [Value];

    private const ushort
        MinValue = 1,
        MaxValue = 5;

    public static ErrOrNothing CheckValueForErr(ushort value) {
        if (value < MinValue) {
            return ErrFactory.IncorrectFormat(
                $"Rating value cannot be less than {MinValue}"
            );
        }

        if (value > MaxValue) {
            return ErrFactory.IncorrectFormat(
                $"Rating value cannot be greater than {MaxValue}"
            );
        }

        return ErrOrNothing.Nothing;
    }


    public static ErrOr<RatingValue> Create(ushort value) =>
        CheckValueForErr(value).IsErr(out var err) ? err : new RatingValue(value);
}