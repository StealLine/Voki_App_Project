using SharedKernel.common.app_users;
using UserProfilesService.Application.common.repositories;

namespace UserProfilesService.Api.contracts;

public record class ListUsersToInviteResponse(
    SingleUserToInviteResponse[] Users
) : ICreatableResponse<UserPreviewWithAllowInvitesSettingDto[]>
{
    public static ICreatableResponse<UserPreviewWithAllowInvitesSettingDto[]> Create(
        UserPreviewWithAllowInvitesSettingDto[] users
    ) => new ListUsersToInviteResponse(
        users.Select(SingleUserToInviteResponse.Create).ToArray()
    );
}

public record SingleUserToInviteResponse(
    string Id,
    string UniqueName,
    string DisplayName,
    string ProfilePic,
    AllowCoAuthorInvitesSettingValue AllowCoAuthorInvites
)
{
    public static SingleUserToInviteResponse Create(UserPreviewWithAllowInvitesSettingDto dto) => new(
        dto.UserId.ToString(),
        dto.UniqueName.ToString(),
        dto.DisplayName.ToString(),
        dto.UserProfilePicKey.ToString(),
        dto.AllowCoAuthorInvites
    );
}