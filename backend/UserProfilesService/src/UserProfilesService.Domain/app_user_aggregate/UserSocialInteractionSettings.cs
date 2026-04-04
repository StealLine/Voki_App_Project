using SharedKernel.common.app_users;

namespace UserProfilesService.Domain.app_user_aggregate;

public record class UserSocialInteractionSettings(
    AllowCoAuthorInvitesSettingValue AllowCoAuthorInvites
)
{
    public static UserSocialInteractionSettings Default => new UserSocialInteractionSettings(
        AllowCoAuthorInvitesSettingValueExtensions.Default
    );
}