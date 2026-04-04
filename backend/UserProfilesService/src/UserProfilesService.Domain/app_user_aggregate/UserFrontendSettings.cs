namespace UserProfilesService.Domain.app_user_aggregate;

public record class UserFrontendSettings(
    VokiTakingSettings VokiTakingSettings,
    VokiFlagsSettings FlagsSettings
)
{
    public static UserFrontendSettings Default() => new(VokiTakingSettings.Default(), VokiFlagsSettings.Default());
}

public enum LanguageFlagDisplayType
{
    Flag,
    ThreeLetterCode
}

public record VokiFlagsSettings(
    bool ShowSignInOnlyFlag,
    bool ShowHasMatureContentFlag,
    LanguageFlagDisplayType LanguageDisplay
)
{
    public static VokiFlagsSettings Default() => new(true, true, LanguageFlagDisplayType.Flag);
}

public record VokiTakingSettings(
    GeneralVokiTakingSettings General
)
{
    public static VokiTakingSettings Default() => new(GeneralVokiTakingSettings.Default());
}

public record GeneralVokiTakingSettings(
    bool AllowArrowNavigationBetweenQuestions,
    bool AllowArrowNavigationBetweenAnswers
)
{
    public static GeneralVokiTakingSettings Default() => new(true, true);
}