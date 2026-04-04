using UserProfilesService.Application.dtos;
using UserProfilesService.Domain.app_user_aggregate.profile_settings;

namespace UserProfilesService.Api.contracts.shared;

public record UserProfileSettingsResponse(
    IUserBannerPrimitiveDto Banner,
    UserProfileTextFieldSettingResponse Status,
    UserProfileTextFieldSettingResponse Pronouns,
    UserProfileTextFieldSettingResponse AboutMe,
    UserLinksSettingResponse Links
)
{
    public static UserProfileSettingsResponse Create(UserProfileSettings s) => new(
        Banner: IUserBannerPrimitiveDto.Create(s.Banner),
        Status: UserProfileTextFieldSettingResponse.Create(s.Status.ShowOnProfile, s.Status.Value),
        Pronouns: UserProfileTextFieldSettingResponse.Create(s.Pronouns.ShowOnProfile, s.Pronouns.Value),
        AboutMe: UserProfileTextFieldSettingResponse.Create(s.AboutMe.ShowOnProfile, s.AboutMe.Value),
        Links: UserLinksSettingResponse.Create(s.Links)
    );
}

public record UserProfileTextFieldSettingResponse(bool ShowOnProfile, string Value)
{
    public static UserProfileTextFieldSettingResponse Create(bool showOnProfile, string value) =>
        new(showOnProfile, value);
}

public record UserLinksSettingResponse(bool ShowOnProfile, UserProfileLinkResponse[] Links)
{
    public static UserLinksSettingResponse Create(UserLinksSetting s) => new(
        s.ShowOnProfile,
        s.Links.Select(l => new UserProfileLinkResponse(l.Value, l.Type)).ToArray()
    );
}
