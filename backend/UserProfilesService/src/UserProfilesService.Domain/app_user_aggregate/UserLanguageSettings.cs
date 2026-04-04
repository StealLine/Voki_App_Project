using SharedKernel.exceptions;

namespace UserProfilesService.Domain.app_user_aggregate;

public sealed class UserLanguageSettings : ValueObject
{
    private UserLanguageSettings() { }
    public bool ShowOnProfile { get; }
    private readonly HashSet<Language> _knownLanguages;
    public IReadOnlySet<Language> KnownLanguages => _knownLanguages;
    public UnknownLanguagesSettings UnknownLanguages { get; }

    private UserLanguageSettings(
        HashSet<Language> knownLanguages,
        bool showOnProfile,
        UnknownLanguagesSettings unknownLanguages
    ) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(knownLanguages, unknownLanguages));
        _knownLanguages = knownLanguages;
        ShowOnProfile = showOnProfile;
        UnknownLanguages = unknownLanguages;
    }

    public static UserLanguageSettings Default() => new(
        [],
        false,
        new UnknownLanguagesSettings(UnknownLanguagesSettingsValue.HideOnlyBlacklist, [])
    );

    public static ErrOr<UserLanguageSettings> Create(
        bool showOnProfile,
        HashSet<Language> knownLanguages,
        UnknownLanguagesSettings unknownLanguages
    ) =>
        CheckForErr(knownLanguages, unknownLanguages).IsErr(out var err)
            ? err
            : new UserLanguageSettings(knownLanguages, showOnProfile, unknownLanguages);

    public static ErrOrNothing CheckForErr(
        HashSet<Language> knownLanguages,
        UnknownLanguagesSettings unknownLanguages
    ) {
        if (knownLanguages.Intersect(unknownLanguages.Blacklist).Any()) {
            return ErrFactory.Conflict("Known languages and blacklisted languages cannot overlap");
        }

        return ErrOrNothing.Nothing;
    }

    public override IEnumerable<object> GetEqualityComponents() => [
        _knownLanguages,
        ShowOnProfile,
        UnknownLanguages
    ];
}

public record UnknownLanguagesSettings
{
    private UnknownLanguagesSettings() { }
    public UnknownLanguagesSettingsValue Value { get; }
    private readonly HashSet<Language> _blacklist;
    public IReadOnlySet<Language> Blacklist => _blacklist;

    public UnknownLanguagesSettings(UnknownLanguagesSettingsValue value, HashSet<Language> blacklist) {
        Value = value;
        _blacklist = blacklist;
    }
}

public enum UnknownLanguagesSettingsValue
{
    HideAllUnknown,
    HideOnlyBlacklist
}