using UserProfilesService.Api.contracts.shared;
using UserProfilesService.Domain.app_user_aggregate;

namespace UserProfilesService.Api.contracts;

public sealed record AllUserSettingsResponse(
    string UniqueName,
    string DisplayName,
    UserProfilePicResponse ProfilePic,
    UserLanguageSettingsResponse LanguageSettings,
    UserFavoriteTagsSettingResponse FavoriteTagsSetting,
    UserFeaturedAuthorsSettingResponse FeaturedAuthorsSetting,
    UserFrontendSettingsResponse FrontendSettings,
    UserProfileSettingsResponse ProfileSettings,
    UserSocialInteractionSettingsResponse SocialInteractionSettings
) : ICreatableResponse<AppUser>
{
    public static ICreatableResponse<AppUser> Create(AppUser u) =>
    new AllUserSettingsResponse(
            UniqueName: u.UniqueName.ToString(),
            DisplayName: u.DisplayName.ToString(),
            ProfilePic: UserProfilePicResponse.Create(u.ProfilePic),
            LanguageSettings: UserLanguageSettingsResponse.Create(u.LanguageSettings),
            FavoriteTagsSetting: UserFavoriteTagsSettingResponse.Create(u.FavoriteTagsSetting),
            FeaturedAuthorsSetting: UserFeaturedAuthorsSettingResponse.Create(u.FeaturedAuthorsSetting),
            FrontendSettings: UserFrontendSettingsResponse.Create(u.FrontendSettings),
            ProfileSettings: UserProfileSettingsResponse.Create(u.ProfileSettings),
            SocialInteractionSettings: UserSocialInteractionSettingsResponse.Create(u.SocialInteractionSettings)
        );
}