using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using UserProfilesService.Application.common.repositories;
using UserProfilesService.Domain.app_user_aggregate;

namespace UserProfilesService.Application.app_users.queries;

public sealed record GetCurrentUserQuery :
    IQuery<AppUser>,
    IWithAuthCheckStep
{
    public Err UnauthenticatedErr => ErrFactory.AuthRequired("Could not get current user because user is not logged in");
}

internal sealed class GetCurrentUserQueryHandler : IQueryHandler<GetCurrentUserQuery, AppUser>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public GetCurrentUserQueryHandler(
        IAppUsersRepository appUsersRepository,
        IUserCtxProvider userCtxProvider
    ) {
        _appUsersRepository = appUsersRepository;
        _userCtxProvider = userCtxProvider;
    }


    public async Task<ErrOr<AppUser>> Handle(GetCurrentUserQuery query, CancellationToken ct) {
        var aCtx = query.UserCtx(_userCtxProvider);
        AppUser? user = await _appUsersRepository.GetCurrentUser(aCtx, ct);

        if (user is null) {
            return ErrFactory.NotFound.User(
                "Current user was not found in the database",
                $"There is no users with id {aCtx.UserId}"
            );
        }

        return user;
    }
}