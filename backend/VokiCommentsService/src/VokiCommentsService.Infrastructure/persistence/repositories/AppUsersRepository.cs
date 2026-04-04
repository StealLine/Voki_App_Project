using VokiCommentsService.Application.common.repositories;
using VokiCommentsService.Domain.app_user_aggregate;

namespace VokiCommentsService.Infrastructure.persistence.repositories;

internal class AppUsersRepository : IAppUsersRepository
{
    private readonly VokiCommentsDbContext _db;

    public AppUsersRepository(VokiCommentsDbContext db) {
        _db = db;
    }

    public async Task Add(AppUser user, CancellationToken ct) {
        await _db.AddAsync(user, ct);
        await _db.SaveChangesAsync(ct);
    }
}