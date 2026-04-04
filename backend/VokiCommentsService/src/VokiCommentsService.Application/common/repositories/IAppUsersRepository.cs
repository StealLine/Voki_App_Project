using VokiCommentsService.Domain.app_user_aggregate;

namespace VokiCommentsService.Application.common.repositories;

public interface IAppUsersRepository
{
    Task Add(AppUser user, CancellationToken ct);
}