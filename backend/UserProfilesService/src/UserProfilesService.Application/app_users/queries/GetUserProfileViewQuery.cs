using UserProfilesService.Application.common.repositories;
using UserProfilesService.Domain.app_user_aggregate;
using UserProfilesService.Domain.app_user_aggregate.dtos;

namespace UserProfilesService.Application.app_users.queries;

public sealed record GetUserProfileViewQuery(
    AppUserId UserId
) : IQuery<UserProfileViewDto>;

internal sealed class GetUserProfileViewQueryHandler : IQueryHandler<GetUserProfileViewQuery, UserProfileViewDto>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public GetUserProfileViewQueryHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }


    public async Task<ErrOr<UserProfileViewDto>> Handle(GetUserProfileViewQuery query, CancellationToken ct) {
        AppUser? user = await _appUsersRepository.GetById(query.UserId, ct);
        if (user is null) {
            return ErrFactory.NotFound.User(
                "Requested user was not found",
                $"There is no users with id {query.UserId}"
            );
        }

        return user.GetProfileViewData();
    }
}