using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.common.app_users;
using UserProfilesService.Application.common.repositories;
using UserProfilesService.Domain.app_user_aggregate;
using VokimiStorageKeysLib.concrete_keys.profile_pics;

namespace UserProfilesService.Application.app_users.queries;

public sealed record GetCurrentUserBasicSetupInfoQuery :
    IQuery<GetCurrentUserBasicSetupInfoQueryResult>,
    IWithAuthCheckStep
{
    public Err UnauthenticatedErr =>
        ErrFactory.AuthRequired("Could not get current user because user is not logged in");
}

internal sealed class GetCurrentUserBasicSetupInfoQueryHandler
    : IQueryHandler<GetCurrentUserBasicSetupInfoQuery, GetCurrentUserBasicSetupInfoQueryResult>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public GetCurrentUserBasicSetupInfoQueryHandler(
        IAppUsersRepository appUsersRepository,
        IUserCtxProvider userCtxProvider
    ) {
        _appUsersRepository = appUsersRepository;
        _userCtxProvider = userCtxProvider;
    }


    public async Task<ErrOr<GetCurrentUserBasicSetupInfoQueryResult>> Handle(
        GetCurrentUserBasicSetupInfoQuery query, CancellationToken ct
    ) {
        var aCtx = query.UserCtx(_userCtxProvider);
        AppUser? user = await _appUsersRepository.GetCurrentUser(aCtx, ct);

        if (user is null) {
            return ErrFactory.NotFound.User(
                "Current user was not found in the database",
                $"There is no users with id {aCtx.UserId}"
            );
        }

        return new GetCurrentUserBasicSetupInfoQueryResult(
            user.UniqueName,
            user.DisplayName,
            user.LanguageSettings.KnownLanguages.ToArray(),
            user.FavoriteTagsSetting.Tags,
            user.ProfilePic.Key
        );
    }
}

public record GetCurrentUserBasicSetupInfoQueryResult(
    UserUniqueName UniqueName,
    UserDisplayName DisplayName,
    Language[] KnownLanguages,
    ImmutableHashSet<VokiTagId> FavoriteTags,
    UserProfilePicKey ProfilePicKey
);