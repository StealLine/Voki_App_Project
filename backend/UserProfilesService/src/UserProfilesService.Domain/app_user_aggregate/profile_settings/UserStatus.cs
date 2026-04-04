using SharedKernel.exceptions;

namespace UserProfilesService.Domain.app_user_aggregate.profile_settings;

public sealed class UserStatus : ValueObject
{
    public bool ShowOnProfile { get; }
    public string Value { get; }
    public const int MaxLength = 250;

    private UserStatus(bool showOnProfile, string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(showOnProfile, value));
        ShowOnProfile = showOnProfile;
        Value = value;
    }

    public static UserStatus Default() => new(false, string.Empty);

    public static ErrOr<UserStatus> Create(bool enabled, string value) =>
        CheckForErr(enabled, value).IsErr(out var err)
            ? err
            : new UserStatus(enabled, value);

    public static ErrOrNothing CheckForErr(bool enabled, string value) {
        if (enabled && string.IsNullOrWhiteSpace(value)) {
            return ErrFactory.NoValue.Common("Status cannot be empty when enabled");
        }

        if (value.Length > MaxLength) {
            return ErrFactory.IncorrectFormat($"Status cannot exceed {MaxLength} characters even when disabled");
        }

        return ErrOrNothing.Nothing;
    }

    public override IEnumerable<object> GetEqualityComponents() => [ShowOnProfile, Value];
    public override string ToString() => ShowOnProfile ? Value : "(Hidden)";
}