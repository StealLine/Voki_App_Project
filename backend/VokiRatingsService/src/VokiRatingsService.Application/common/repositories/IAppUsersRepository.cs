using SharedKernel.user_ctx;
using VokiRatingsService.Domain.app_user_aggregate;

namespace VokiRatingsService.Application.common.repositories;

public interface IAppUsersRepository
{
    Task Add(AppUser user, CancellationToken ct);
    Task<AppUser?> GetByIdForUpdate(AppUserId userId, CancellationToken ct);
    Task<AppUser?> GetCurrentForUpdate(AuthenticatedUserCtx aUserCtx, CancellationToken ct);
    Task<AppUser?> GetCurrent(AuthenticatedUserCtx aUserCtx, CancellationToken ct);
    Task Update(AppUser appUser, CancellationToken ct);
}