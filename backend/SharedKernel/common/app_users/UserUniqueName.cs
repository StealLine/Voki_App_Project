using System.Text.RegularExpressions;

namespace SharedKernel.common.app_users;

public class UserUniqueName : ValueObject
{
    public readonly string Value;

    private const int MinLength = 6;
    public const int MaxLength = 24;

    private static readonly Regex AllowedCharsRegex = new(
        "^[A-Za-z0-9._~-]+$", RegexOptions.Compiled
    );

    private static readonly Regex SingleCharAllowedRegex = new(
        "^[A-Za-z0-9._~-]$", RegexOptions.Compiled
    );

    private const string AllowedCharsDescription =
        "letters (A–Z, a–z), digits (0–9), hyphen (-), dot (.), underscore (_), and tilde (~)";

    public UserUniqueName(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(value));
        Value = value;
    }

    public override string ToString() => Value;

    public override IEnumerable<object> GetEqualityComponents() => [Value];

    public static ErrOr<UserUniqueName> Create(string value) =>
        CheckForErr(value).IsErr(out var err) ? err : new UserUniqueName(value);

    public static ErrOrNothing CheckForErr(string name) {
        if (string.IsNullOrWhiteSpace(name)) {
            return ErrFactory.NoValue.Common("Provided value is empty");
        }

        if (name.Length < MinLength || name.Length > MaxLength) {
            return ErrFactory.IncorrectFormat($"Name must be between {MinLength} and {MaxLength} characters");
        }

        if (name.Any(char.IsWhiteSpace)) {
            return ErrFactory.IncorrectFormat("Name cannot contain whitespace characters");
        }

        if (!AllowedCharsRegex.IsMatch(name)) {
            var invalidChars = name
                .Where(c => !SingleCharAllowedRegex.IsMatch(c.ToString()))
                .Distinct()
                .Select(c => $"'{c}'");

            var details =
                $"Invalid characters: {string.Join(", ", invalidChars)}. Allowed characters are: {AllowedCharsDescription}";

            return ErrFactory.IncorrectFormat("Name contains invalid characters", details);
        }

        return ErrOrNothing.Nothing;
    }
}