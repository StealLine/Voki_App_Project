using InfrastructureShared.EfCore;
using InfrastructureShared.EfCore.db_extensions;
using Microsoft.EntityFrameworkCore;
using SharedKernel.user_ctx;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.app_user_aggregate;

namespace VokiRatingsService.Infrastructure.persistence.repositories;

internal class AppUsersRepository : IAppUsersRepository
{
    private readonly VokiRatingsDbContext _db;

    public AppUsersRepository(VokiRatingsDbContext db) {
        _db = db;
    }

    public async Task Add(AppUser user, CancellationToken ct) {
        await _db.AppUsers.AddAsync(user, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<AppUser?> GetByIdForUpdate(AppUserId userId, CancellationToken ct) =>
        await _db.FindByIdForUpdateAsync<AppUser, AppUserId>(userId, ct);

    public Task<AppUser?> GetCurrentForUpdate(AuthenticatedUserCtx aUserCtx, CancellationToken ct) =>
        _db.FindByIdForUpdateAsync<AppUser, AppUserId>(aUserCtx.UserId, ct);

    public Task<AppUser?> GetCurrent(AuthenticatedUserCtx aUserCtx, CancellationToken ct) => _db.AppUsers
        .FirstOrDefaultAsync(u => u.Id == aUserCtx.UserId, ct);

    public async Task Update(AppUser appUser, CancellationToken ct) {
        _db.ThrowIfDetached(appUser);
        _db.AppUsers.Update(appUser);
        await _db.SaveChangesAsync(ct);
    }
}