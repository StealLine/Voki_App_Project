using AlbumsService.Application.common.repositories;
using AlbumsService.Domain.app_user_aggregate;
using InfrastructureShared.EfCore;
using InfrastructureShared.EfCore.db_extensions;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using SharedKernel.user_ctx;

namespace AlbumsService.Infrastructure.persistence.repositories;

internal class AppUsersRepository : IAppUsersRepository
{
    private readonly AlbumsDbContext _db;

    public AppUsersRepository(AlbumsDbContext db) {
        _db = db;
    }

    public async Task Add(AppUser user, CancellationToken ct) {
        await _db.AppUsers.AddAsync(user, ct);
        await _db.SaveChangesAsync(ct);
    }


    public Task<AppUser?> GetCurrentForUpdate(AuthenticatedUserCtx ctx, CancellationToken ct) =>
        _db.FindByIdForUpdateAsync<AppUser, AppUserId>(ctx.UserId, ct);

    public async Task Update(AppUser user, CancellationToken ct) {
        _db.ThrowIfDetached(user);
        _db.AppUsers.Update(user);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<UserAutoAlbumsAppearance?> GetCurrentUserAutoAlbumsAppearance(
        AuthenticatedUserCtx ctx,
        CancellationToken ct
    ) =>
        await _db.AppUsers
            .Select(u => new { u.Id, u.AutoAlbumsAppearance })
            .FirstOrDefaultAsync(u => u.Id == ctx.UserId, ct)
            .Select(u => u?.AutoAlbumsAppearance ?? null);
}