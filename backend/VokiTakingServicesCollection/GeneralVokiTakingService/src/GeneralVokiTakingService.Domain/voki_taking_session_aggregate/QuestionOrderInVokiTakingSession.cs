using SharedKernel.exceptions;

namespace GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

public class QuestionOrderInVokiTakingSession : ValueObject, IComparable
{
    public ushort Value { get; }

    private QuestionOrderInVokiTakingSession(ushort value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(value));
        Value = value;
    }

    public static ErrOr<QuestionOrderInVokiTakingSession> Create(int value) =>
        CheckForErr(value).IsErr(out var err) ? err : new QuestionOrderInVokiTakingSession((ushort)value);

    private static ErrOrNothing CheckForErr(int val) => val switch {
        < 1 => ErrFactory.ValueOutOfRange("Question order in voki taking session must start with 1"),
        > ushort.MaxValue => ErrFactory.ValueOutOfRange(
            $"Question order in voki taking session cannot be greater than {ushort.MaxValue}"
        ),
        _ => ErrOrNothing.Nothing
    };

    public override IEnumerable<object> GetEqualityComponents() => [Value];
    public override string ToString() => Value.ToString();
    public int CompareTo(object? obj) => Value.CompareTo(obj is QuestionOrderInVokiTakingSession other ? other.Value : null);
}