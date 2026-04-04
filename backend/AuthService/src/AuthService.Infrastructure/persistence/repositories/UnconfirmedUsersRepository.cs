using AuthService.Application.common.repositories;
using AuthService.Domain.unconfirmed_user_aggregate;
using InfrastructureShared.EfCore;
using InfrastructureShared.EfCore.db_extensions;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.persistence.repositories;

internal sealed class UnconfirmedUsersRepository : IUnconfirmedUsersRepository
{
    private readonly AuthDbContext _db;

    public UnconfirmedUsersRepository(AuthDbContext db) {
        _db = db;
    }

    public Task<UnconfirmedUser?> GetByEmailForUpdate(Email email, CancellationToken ct) =>
        _db.FindForUpdateAsync<UnconfirmedUser>(u => u.Email == email, ct: ct);

    public Task<UnconfirmedUser?> GetByIdForUpdate(
        UnconfirmedUserId userId,
        CancellationToken ct
    ) =>
        _db.FindByIdForUpdateAsync<UnconfirmedUser, UnconfirmedUserId>(userId, ct);

    public async Task Add(UnconfirmedUser unconfirmedUser, CancellationToken ct) {
        await _db.UnconfirmedUsers.AddAsync(unconfirmedUser, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task Update(UnconfirmedUser unconfirmedUser, CancellationToken ct) {
        _db.ThrowIfDetached(unconfirmedUser);
        _db.UnconfirmedUsers.Update(unconfirmedUser);
        await _db.SaveChangesAsync(ct);
    }

    public async Task Delete(UnconfirmedUser unconfirmedUser, CancellationToken ct) {
        _db.ThrowIfDetached(unconfirmedUser);
        _db.UnconfirmedUsers.Remove(unconfirmedUser);
        await _db.SaveChangesAsync(ct);
    }

    public Task<int> DeleteAllExpiredUsers(DateTime utcNow, CancellationToken ct) =>
        _db.UnconfirmedUsers
            .Where(u => u.ExpiresAt <= utcNow)
            .ExecuteDeleteAsync(ct);
}