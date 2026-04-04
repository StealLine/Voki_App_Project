using System.Text.RegularExpressions;
using SharedKernel.common.app_users;
using SharedKernel.exceptions;

namespace UserProfilesService.Domain.app_user_aggregate;

public class UserDisplayName : ValueObject
{
    private readonly string _value;

    private const int MinLength = 4;
    public const int MaxLength = 30;

    private static readonly Regex AllowedCharsRegex = new(
        @"^[\p{L}\p{M}\p{N}\p{Zs}\-._'’()!¡\?¿]+$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant
    );


    private UserDisplayName(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(value));
        _value = value;
    }


    public override string ToString() => _value;
    public override IEnumerable<object> GetEqualityComponents() => [_value];

    public static UserDisplayName FromUniqueName(UserUniqueName uniqueName) =>
        new(uniqueName.Value);

    public static ErrOr<UserDisplayName> Create(string value) =>
        CheckForErr(value).IsErr(out var err) ? err : new UserDisplayName(value);

    public static ErrOrNothing CheckForErr(string? name) {
        if (string.IsNullOrWhiteSpace(name)) {
            return ErrFactory.NoValue.Common("Name is required");
        }

        if (name.Length < MinLength || name.Length > MaxLength) {
            return ErrFactory.IncorrectFormat($"Name must be empty or between {MinLength} and {MaxLength} characters");
        }

        if (char.IsWhiteSpace(name[0]) || char.IsWhiteSpace(name[^1])) {
            return ErrFactory.IncorrectFormat("Name cannot start or end with a space");
        }

        if (name.Contains("  ")) {
            return ErrFactory.IncorrectFormat("Name cannot contain consecutive spaces");
        }

        if (!AllowedCharsRegex.IsMatch(name)) {
            return ErrFactory.IncorrectFormat("Name contains invalid characters");
        }

        return ErrOrNothing.Nothing;
    }
}