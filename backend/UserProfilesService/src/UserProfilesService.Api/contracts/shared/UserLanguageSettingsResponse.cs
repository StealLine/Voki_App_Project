namespace UserProfilesService.Api.contracts.shared;

public record class UserLanguageSettingsResponse(
    bool ShowOnProfile,
    Language[] KnownLanguages,
    UnknownLanguagesSettingsValue UnknownLanguagesSettingsValue,
    Language[] UnknownLanguagesBlacklist
)
{
    public static UserLanguageSettingsResponse Create(UserLanguageSettings settings) => new(
        settings.ShowOnProfile,
        settings.KnownLanguages.ToArray(),
        settings.UnknownLanguages.Value,
        settings.UnknownLanguages.Blacklist.ToArray()
    );
}