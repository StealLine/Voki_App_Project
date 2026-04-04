using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.app_user_aggregate;
using InfrastructureShared.EfCore;
using InfrastructureShared.EfCore.db_extensions;
using Microsoft.EntityFrameworkCore;
using SharedKernel.user_ctx;

namespace CoreVokiCreationService.Infrastructure.persistence.repositories;

internal class AppUsersRepository : IAppUsersRepository
{
    private readonly CoreVokiCreationDbContext _db;

    public AppUsersRepository(CoreVokiCreationDbContext db) {
        _db = db;
    }

    public Task<AppUser?> GetByIdForUpdate(AppUserId id, CancellationToken ct) =>
        _db.FindByIdForUpdateAsync<AppUser, AppUserId>(id, ct);

    public Task<AppUser[]> ListWithIdsForUpdate(IEnumerable<AppUserId> userIds, CancellationToken ct) =>
        _db.ListForUpdateAsync<AppUser>(u => userIds.Contains(u.Id), ct);

    public async Task Add(AppUser user, CancellationToken ct) {
        await _db.AppUsers.AddAsync(user, ct);
        await _db.SaveChangesAsync(ct);
    }

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

    public Task<AppUser?> GetCurrent(AuthenticatedUserCtx authenticatedUserCtx, CancellationToken ct) =>
        _db.AppUsers.FirstOrDefaultAsync(u => u.Id == authenticatedUserCtx.UserId, cancellationToken: ct);
}