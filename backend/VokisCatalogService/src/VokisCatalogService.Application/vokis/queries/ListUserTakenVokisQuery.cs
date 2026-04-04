using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using VokisCatalogService.Application.common.repositories;
using VokisCatalogService.Domain.app_user_aggregate;

namespace VokisCatalogService.Application.vokis.queries;

public sealed record ListUserTakenVokisQuery() :
    IQuery<UserTakenVokisList>,
    IWithAuthCheckStep;

internal sealed class ListUserTakenVokisQueryHandler : IQueryHandler<ListUserTakenVokisQuery, UserTakenVokisList>
{
    private readonly IUserCtxProvider _userCtxProvider;
    private readonly IAppUsersRepository _appUsersRepository;

    public ListUserTakenVokisQueryHandler(IUserCtxProvider userCtxProvider, IAppUsersRepository appUsersRepository) {
        _userCtxProvider = userCtxProvider;
        _appUsersRepository = appUsersRepository;
    }

    public async Task<ErrOr<UserTakenVokisList>> Handle(ListUserTakenVokisQuery query, CancellationToken ct) {
        var curCtx = query.UserCtx(_userCtxProvider);
        AppUser? user = await _appUsersRepository.GetCurrentUserWithTakenVokis(curCtx, ct);
        if (user is null) {
            return ErrFactory.NotFound.User("Cannot find user account", $"User id: {curCtx.UserId}");
        }

        return user.TakenVokis;
    }
}