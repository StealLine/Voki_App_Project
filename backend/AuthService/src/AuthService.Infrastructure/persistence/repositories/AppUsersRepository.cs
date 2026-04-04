using AuthService.Application.common.repositories;
using AuthService.Domain.app_user_aggregate;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.persistence.repositories;

internal sealed class AppUsersRepository : IAppUsersRepository
{
    private readonly AuthDbContext _db;

    public AppUsersRepository(AuthDbContext db) {
        _db = db;
    }

    public Task<bool> AnyUserWithId(AppUserId appUserId, CancellationToken ct) =>
        _db.AppUsers.AnyAsync(u => u.Id == appUserId, ct);

    public Task<bool> AnyUserWithEmail(Email email, CancellationToken ct) =>
        _db.AppUsers.AnyAsync(u => u.Email == email, ct);

    public Task<AppUser?> GetByEmail(Email email, CancellationToken ct) =>
        _db.AppUsers.FirstOrDefaultAsync(u => u.Email == email, ct);

    public async Task Add(AppUser user, CancellationToken ct) {
        await _db.AppUsers.AddAsync(user, ct);
        await _db.SaveChangesAsync(ct);
    }
}