using System.Text.RegularExpressions;

namespace SharedKernel.domain.ids;

public class VokiTagId : ValueObject, IEntityId
{
    public const int MaxTagLength = 30;

    public static readonly Regex TagRegex =
        new Regex(@"^[a-zA-Zа-яА-Я0-9\+\-_]{1," + MaxTagLength + "}$");

    public static bool IsStringValidTag(string tag) => TagRegex.IsMatch(tag);
    public string Value { get; }

    public VokiTagId(string value)
    {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(value));
        Value = value;
    }

    public static ErrOrNothing CheckForErr(string tag)
    {
        if (!IsStringValidTag(tag))
        {
            return ErrFactory.IncorrectFormat($"'{tag}' is not a valid tag");
        }

        return ErrOrNothing.Nothing;
    }

    public static ErrOr<VokiTagId> Create(string value) =>
        CheckForErr(value).IsErr(out var err) ? err : new VokiTagId(value);

    public override string ToString() => Value;

    public int CompareTo(object? obj) => obj switch
    {
        IEntityId ed => ToString().CompareTo(ed.ToString()),
        _ => -1
    };

    public override IEnumerable<object> GetEqualityComponents() => [Value];
}