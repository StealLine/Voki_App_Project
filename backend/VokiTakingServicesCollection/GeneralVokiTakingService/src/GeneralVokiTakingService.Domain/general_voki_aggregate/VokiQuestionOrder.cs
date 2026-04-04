using SharedKernel.exceptions;

namespace GeneralVokiTakingService.Domain.general_voki_aggregate;

public class VokiQuestionOrder : ValueObject
{
    public ushort Value { get; }

    private VokiQuestionOrder(ushort value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(value));
        Value = value;
    }

    public static ErrOr<VokiQuestionOrder> Create(ushort value) =>
        CheckForErr(value).IsErr(out var err) ? err : new VokiQuestionOrder(value);

    private static ErrOrNothing CheckForErr(ushort val) => val switch {
        < 1 => ErrFactory.ValueOutOfRange("Question order in voki must start with 1"),
        _ => ErrOrNothing.Nothing
    };

    public override IEnumerable<object> GetEqualityComponents() => [Value];
    public override string ToString() => Value.ToString();
}