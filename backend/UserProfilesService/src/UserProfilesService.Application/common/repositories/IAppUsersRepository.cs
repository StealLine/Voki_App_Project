using SharedKernel.common.app_users;
using SharedKernel.user_ctx;
using UserProfilesService.Domain.app_user_aggregate;
using VokimiStorageKeysLib.concrete_keys;
using VokimiStorageKeysLib.concrete_keys.profile_pics;

namespace UserProfilesService.Application.common.repositories;

public interface IAppUsersRepository
{
    Task<AppUser?> GetByIdForUpdate(AppUserId userId, CancellationToken ct);
    Task Add(AppUser user, CancellationToken ct);
    Task Update(AppUser user, CancellationToken ct);
    Task<AppUser?> GetById(AppUserId userId, CancellationToken ct);
    Task<AppUser?> GetCurrentUser(AuthenticatedUserCtx authenticatedUserCtx, CancellationToken ct);

    public Task<UserPreviewDto[]> GetUserNamesWithProfilePics(IEnumerable<AppUserId> userIds, CancellationToken ct);

    Task<UserPreviewWithAllowInvitesSettingDto[]> SearchToInviteByNameQuery(string searchValue, int limit, CancellationToken ct);
    Task<UserPreviewWithAllowInvitesSettingDto[]> ListAllUsers(CancellationToken ct);
}

public record UserPreviewDto(
    AppUserId UserId,
    UserUniqueName UniqueName,
    UserDisplayName DisplayName,
    UserProfilePic UserProfilePicKey
);

public record UserPreviewWithAllowInvitesSettingDto(
    AppUserId UserId,
    UserUniqueName UniqueName,
    UserDisplayName DisplayName,
    UserProfilePic UserProfilePicKey,
    AllowCoAuthorInvitesSettingValue AllowCoAuthorInvites
);