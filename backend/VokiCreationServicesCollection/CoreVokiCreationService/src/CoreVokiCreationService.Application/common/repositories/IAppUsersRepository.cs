using CoreVokiCreationService.Domain.app_user_aggregate;
using SharedKernel.user_ctx;

namespace CoreVokiCreationService.Application.common.repositories;

public interface IAppUsersRepository
{
    Task Add(AppUser user, CancellationToken ct);
    Task<AppUser?> GetByIdForUpdate(AppUserId id, CancellationToken ct);
    Task<AppUser[]> ListWithIdsForUpdate(IEnumerable<AppUserId> userIds, CancellationToken ct);
    Task Update(AppUser user, CancellationToken ct);
    Task<AppUser?> GetCurrent(AuthenticatedUserCtx authenticatedUserCtx, CancellationToken ct);
    Task UpdateRange(IEnumerable<AppUser> users, CancellationToken ct);
}