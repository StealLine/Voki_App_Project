using SharedKernel.exceptions;

namespace UserProfilesService.Domain.app_user_aggregate.profile_settings;

public sealed class UserAboutMe : ValueObject
{
    public bool ShowOnProfile { get; }
    public string Value { get; }
    public const int MaxLength = 1000;

    private UserAboutMe(bool showOnProfile, string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(showOnProfile, value));
        ShowOnProfile = showOnProfile;
        Value = value;
    }

    public static UserAboutMe Default() => new(false, string.Empty);

    public static ErrOr<UserAboutMe> Create(bool showOnProfile, string value) =>
        CheckForErr(showOnProfile, value).IsErr(out var err) ? err : new UserAboutMe(showOnProfile, value);

    public static ErrOrNothing CheckForErr(bool enabled, string value) {
        if (enabled && string.IsNullOrWhiteSpace(value)) {
            return ErrFactory.NoValue.Common("Value cannot be empty when enabled");
        }

        if (value.Length > MaxLength) {
            return ErrFactory.IncorrectFormat($"Cannot exceed {MaxLength} characters");
        }

        return ErrOrNothing.Nothing;
    }

    public override IEnumerable<object> GetEqualityComponents() => [ShowOnProfile, Value];
    public override string ToString() => ShowOnProfile ? Value : "(Hidden)";
}