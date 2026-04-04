using GeneralVokiTakingService.Application.common.repositories;
using GeneralVokiTakingService.Domain.app_user_aggregate;
using InfrastructureShared.EfCore;
using InfrastructureShared.EfCore.db_extensions;
using Microsoft.EntityFrameworkCore;
using SharedKernel.user_ctx;

namespace GeneralVokiTakingService.Infrastructure.persistence.repositories;

internal class AppUsersRepository : IAppUsersRepository
{
    private readonly GeneralVokiTakingDbContext _db;

    public AppUsersRepository(GeneralVokiTakingDbContext db) {
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

    public Task<AppUser?> GetCurrent(AuthenticatedUserCtx aUserCtx, CancellationToken ct) =>
        _db.AppUsers.FirstOrDefaultAsync(u => u.Id == aUserCtx.UserId, ct);

    public async Task<AppUser?> GetById(AppUserId userId, CancellationToken ct) =>
        await _db.AppUsers
            .FirstOrDefaultAsync(u => u.Id == userId, ct);
}