using AuthService.Domain.app_user_aggregate;

namespace AuthService.Application.common.repositories;

public interface IAppUsersRepository
{
    Task<bool> AnyUserWithId(AppUserId appUserId, CancellationToken ct);
    Task Add(AppUser user, CancellationToken ct);
    
    Task<bool> AnyUserWithEmail(Email email, CancellationToken ct);
    Task<AppUser?> GetByEmail(Email email, CancellationToken ct);
}