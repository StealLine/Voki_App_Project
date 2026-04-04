namespace UserProfilesService.Api.contracts.shared;

public record UserFrontendSettingsResponse(
    UserVokiTakingSettingsResponse VokiTakingSettings,
    UserVokiFlagsSettingsResponse FlagsSettings
)
{
    public static UserFrontendSettingsResponse Create(UserFrontendSettings s) => new(
        UserVokiTakingSettingsResponse.Create(s.VokiTakingSettings),
        UserVokiFlagsSettingsResponse.Create(s.FlagsSettings)
    );
}

public record UserVokiTakingSettingsResponse(UserGeneralVokiTakingSettingsResponse General)
{
    public static UserVokiTakingSettingsResponse Create(VokiTakingSettings s) => new(
        UserGeneralVokiTakingSettingsResponse.Create(s.General)
    );
}

public record UserGeneralVokiTakingSettingsResponse(
    bool AllowArrowNavigationBetweenQuestions,
    bool AllowArrowNavigationBetweenAnswers
)
{
    public static UserGeneralVokiTakingSettingsResponse Create(GeneralVokiTakingSettings s) => new(
        s.AllowArrowNavigationBetweenQuestions,
        s.AllowArrowNavigationBetweenAnswers
    );
}

public record UserVokiFlagsSettingsResponse(
    bool ShowSignInOnlyFlag,
    bool ShowHasMatureContentFlag,
    LanguageFlagDisplayType LanguageDisplay
)
{
    public static UserVokiFlagsSettingsResponse Create(VokiFlagsSettings s) => new(
        s.ShowSignInOnlyFlag,
        s.ShowHasMatureContentFlag,
        s.LanguageDisplay
    );
}
