using SharedKernel.exceptions;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;

public class VokiQuestionOrder : ValueObject
{
    public ushort Value { get; }

    private VokiQuestionOrder(int value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(value));
        Value = (ushort)value;
    }

    public static ErrOr<VokiQuestionOrder> Create(int value) =>
        CheckForErr(value).IsErr(out var err) ? err : new VokiQuestionOrder(value);

    private static ErrOrNothing CheckForErr(int val) => val switch {
        < 1 => ErrFactory.ValueOutOfRange("Question order in voki must start with 1"),
        > ushort.MaxValue => ErrFactory.ValueOutOfRange("Question order in voki must be less than " + ushort.MaxValue),
        _ => ErrOrNothing.Nothing
    };

    public override IEnumerable<object> GetEqualityComponents() => [Value];
    public override string ToString() => Value.ToString();

    public ErrOr<VokiQuestionOrder> MinusOne() {
        if (Value == 1) {
            return ErrFactory.ValueOutOfRange("Question order in voki must start with 1");
        }

        return new VokiQuestionOrder(Value - 1);
    }

    public ErrOr<VokiQuestionOrder> PlusOne() {
        if (Value == ushort.MaxValue) {
            return ErrFactory.ValueOutOfRange("Question order in voki must be less than " + ushort.MaxValue);
        }

        return new VokiQuestionOrder(Value + 1);
    }

    public bool IsFirst() => Value == 1;
}