using GeneralVokiTakingService.Domain.app_user_aggregate;
using SharedKernel.user_ctx;

namespace GeneralVokiTakingService.Application.common.repositories;

public interface IAppUsersRepository
{
    Task Add(AppUser user, CancellationToken ct);
    Task<AppUser?> GetByIdForUpdate(AppUserId id, CancellationToken ct);

    Task Update(AppUser user, CancellationToken ct);
    Task<AppUser?> GetCurrent(AuthenticatedUserCtx aUserCtx, CancellationToken ct);
}