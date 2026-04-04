using AlbumsService.Domain.app_user_aggregate;
using SharedKernel.user_ctx;

namespace AlbumsService.Application.common.repositories;

public interface IAppUsersRepository
{
    Task Add(AppUser user, CancellationToken ct);
    Task<AppUser?> GetCurrentForUpdate(AuthenticatedUserCtx ctx, CancellationToken ct);
    Task Update(AppUser user, CancellationToken ct);
    public Task<UserAutoAlbumsAppearance?> GetCurrentUserAutoAlbumsAppearance(AuthenticatedUserCtx ctx, CancellationToken ct);
}