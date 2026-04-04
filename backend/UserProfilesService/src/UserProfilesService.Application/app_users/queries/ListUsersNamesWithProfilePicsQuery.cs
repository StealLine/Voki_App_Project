using UserProfilesService.Application.common.repositories;

namespace UserProfilesService.Application.app_users.queries;

public sealed record ListUsersNamesWithProfilePicsQuery(
    ImmutableHashSet<AppUserId> UserIds
) : IQuery<UserPreviewDto[]>;

internal sealed class ListUsersNamesWithProfilePicsQueryHandler :
    IQueryHandler<ListUsersNamesWithProfilePicsQuery, UserPreviewDto[]>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public ListUsersNamesWithProfilePicsQueryHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }


    public async Task<ErrOr<UserPreviewDto[]>> Handle(ListUsersNamesWithProfilePicsQuery query, CancellationToken ct) =>
        await _appUsersRepository.GetUserNamesWithProfilePics(query.UserIds, ct);
}