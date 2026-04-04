using ApplicationShared;
using SharedKernel.user_ctx;
using UserProfilesService.Application.common.repositories;

namespace UserProfilesService.Application.app_users.queries;

public sealed record ListUsersRecommendedForCoAuthorQuery(
) : IQuery<UserPreviewWithAllowInvitesSettingDto[]>;

internal sealed class
    ListUsersRecommendedForCoAuthorQueryHandler : IQueryHandler<ListUsersRecommendedForCoAuthorQuery,
    UserPreviewWithAllowInvitesSettingDto[]>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public ListUsersRecommendedForCoAuthorQueryHandler(IAppUsersRepository appUsersRepository, IUserCtxProvider userCtxProvider) {
        _appUsersRepository = appUsersRepository;
        _userCtxProvider = userCtxProvider;
    }


    public async Task<ErrOr<UserPreviewWithAllowInvitesSettingDto[]>> Handle(
        ListUsersRecommendedForCoAuthorQuery query, CancellationToken ct
    ) {
        // get user from context
        // return invites recommendations for this specific users
        return await _appUsersRepository.ListAllUsers(ct);
    }
}