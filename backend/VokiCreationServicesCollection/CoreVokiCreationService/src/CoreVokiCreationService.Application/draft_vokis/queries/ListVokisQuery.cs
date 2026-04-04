using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.app_user_aggregate;
using CoreVokiCreationService.Domain.draft_voki_aggregate;
using SharedKernel.user_ctx;

namespace CoreVokiCreationService.Application.draft_vokis.queries;

public sealed record ListVokisQuery(VokiId[] VokiIds) :
    IQuery<DraftVoki[]>,
    IWithAuthCheckStep;

internal sealed class ListVokisQueryHandler : IQueryHandler<ListVokisQuery, DraftVoki[]>
{
    private readonly IDraftVokiRepository _draftVokiRepository;
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public ListVokisQueryHandler(
        IDraftVokiRepository draftVokiRepository, IAppUsersRepository appUsersRepository, IUserCtxProvider userCtxProvider
    ) {
        _draftVokiRepository = draftVokiRepository;
        _appUsersRepository = appUsersRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<DraftVoki[]>> Handle(
        ListVokisQuery query, CancellationToken ct
    ) {
        var vokis = await _draftVokiRepository.GetMultipleById(query.VokiIds, ct);
        var currentUser = query.UserCtx(_userCtxProvider);
        return vokis.Where(v => v.DoesUserHaveAccess(currentUser)).ToArray();
    }
}