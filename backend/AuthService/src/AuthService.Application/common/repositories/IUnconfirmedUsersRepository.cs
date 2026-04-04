using AuthService.Domain.unconfirmed_user_aggregate;

namespace AuthService.Application.common.repositories;

public interface IUnconfirmedUsersRepository
{
    Task<UnconfirmedUser?> GetByEmailForUpdate(Email email, CancellationToken ct);
    Task<UnconfirmedUser?> GetByIdForUpdate(UnconfirmedUserId unconfirmedUserId, CancellationToken ct);
    
    
    Task Add(UnconfirmedUser unconfirmedUser, CancellationToken ct);
    Task Update(UnconfirmedUser unconfirmedUser, CancellationToken ct);
    Task Delete(UnconfirmedUser unconfirmedUser, CancellationToken ct);
    Task<int> DeleteAllExpiredUsers(DateTime utcNow, CancellationToken ct);

  
}