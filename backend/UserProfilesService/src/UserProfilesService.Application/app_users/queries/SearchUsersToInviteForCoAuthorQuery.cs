using ApplicationShared.messaging.pipeline_behaviors;
using UserProfilesService.Application.common.repositories;

namespace UserProfilesService.Application.app_users.queries;

public sealed record SearchUsersToInviteForCoAuthorQuery(
    string SearchValue,
    int Limit
) :
    IQuery<UserPreviewWithAllowInvitesSettingDto[]>,
    IWithBasicValidationStep
{
    public ErrOrNothing Validate() {
        if (string.IsNullOrWhiteSpace(SearchValue)) {
            return ErrFactory.NoValue.Common("No value provided");
        }

        return ErrOrNothing.Nothing;
    }
}

internal sealed class SearchUsersToInviteForCoAuthorQueryHandler :
    IQueryHandler<SearchUsersToInviteForCoAuthorQuery, UserPreviewWithAllowInvitesSettingDto[]>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public SearchUsersToInviteForCoAuthorQueryHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }


    public async Task<ErrOr<UserPreviewWithAllowInvitesSettingDto[]>> Handle(
        SearchUsersToInviteForCoAuthorQuery query, CancellationToken ct
    ) =>
        await _appUsersRepository.SearchToInviteByNameQuery(query.SearchValue, query.Limit, ct);
}