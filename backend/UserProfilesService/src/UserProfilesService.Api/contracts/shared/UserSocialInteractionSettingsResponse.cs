using SharedKernel.common.app_users;

namespace UserProfilesService.Api.contracts.shared;

public record UserSocialInteractionSettingsResponse(AllowCoAuthorInvitesSettingValue AllowCoAuthorInvites)
{
    public static UserSocialInteractionSettingsResponse Create(UserSocialInteractionSettings s) => new(
        s.AllowCoAuthorInvites
    );
}
