using SharedKernel.exceptions;

namespace SharedKernel.domain.ids;

public abstract class GuidBasedId : ValueObject, IEntityId
{
    private GuidBasedId() { }
    public Guid Value { get; }

    protected GuidBasedId(Guid value)
    {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(value));
        Value = value;
    }

    public static ErrOrNothing CheckForErr(Guid value)
    {
        if (value == Guid.Empty)
        {
            return ErrFactory.NoValue.Common("Guid-based ID cannot be empty");
        }

        return ErrOrNothing.Nothing;
    }
    public override string ToString() => Value.ToString();
    public override IEnumerable<object> GetEqualityComponents() => [Value];

    public int CompareTo(object? obj) => obj switch
    {
        IEntityId ed => ToString().CompareTo(ed.ToString()),
        Guid guid => guid.CompareTo(Value),
        _ => -1
    };
}