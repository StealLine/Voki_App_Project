using InfrastructureShared.EfCore;
using InfrastructureShared.EfCore.db_extensions;
using Microsoft.EntityFrameworkCore;
using SharedKernel.user_ctx;
using VokisCatalogService.Application.common.repositories;
using VokisCatalogService.Domain.app_user_aggregate;

namespace VokisCatalogService.Infrastructure.persistence.repositories;

internal class AppUsersRepository : IAppUsersRepository
{
    private readonly VokisCatalogDbContext _db;

    public AppUsersRepository(VokisCatalogDbContext db) {
        _db = db;
    }

    public async Task Add(AppUser user, CancellationToken ct) {
        await _db.AppUsers.AddAsync(user, ct);
        await _db.SaveChangesAsync(ct);
    }

    public Task<AppUser?> GetByIdForUpdate(AppUserId id, CancellationToken ct) =>
        _db.FindByIdForUpdateAsync<AppUser, AppUserId>(id, ct);

    public async Task Update(AppUser user, CancellationToken ct) {
        _db.ThrowIfDetached(user);
        _db.Update(user);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateRange(IEnumerable<AppUser> users, CancellationToken ct) {
        var materialized = users.ToList();

        _db.ThrowIfDetached(materialized);
        _db.UpdateRange(materialized);
        await _db.SaveChangesAsync(ct);
    }

    public Task<AppUser?> GetUserWithTakenVokisForUpdate(AppUserId userId, CancellationToken ct) =>
        _db.FindWithIncludesForUpdateAsync<AppUser, AppUserId>(q => q.Include(u => u.TakenVokis), userId, ct);

    public Task<AppUser?> GetCurrentUserWithTakenVokis(AuthenticatedUserCtx aUserCtx, CancellationToken ct) =>
        _db.AppUsers
            .Include(u => u.TakenVokis)
            .FirstOrDefaultAsync(u => u.Id == aUserCtx.UserId, ct);
}