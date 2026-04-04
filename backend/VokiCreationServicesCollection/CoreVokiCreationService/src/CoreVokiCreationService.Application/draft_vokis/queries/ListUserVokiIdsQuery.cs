using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using CoreVokiCreationService.Application.common.repositories;
using SharedKernel.user_ctx;

namespace CoreVokiCreationService.Application.draft_vokis.queries;

public sealed record ListUserVokiIdsQuery() :
    IQuery<ImmutableArray<VokiId>>,
    IWithAuthCheckStep;

internal sealed class ListUserVokiIdsQueryHandler : IQueryHandler<ListUserVokiIdsQuery, ImmutableArray<VokiId>>
{
    private readonly IDraftVokiRepository _draftVokiRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public ListUserVokiIdsQueryHandler(IDraftVokiRepository draftVokiRepository, IUserCtxProvider userCtxProvider) {
        _draftVokiRepository = draftVokiRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<ImmutableArray<VokiId>>> Handle(ListUserVokiIdsQuery query, CancellationToken ct) =>
    (
        await _draftVokiRepository.ListVokiAuthoredByUserOrderByCreationDate(query.UserCtx(_userCtxProvider), ct)
    ).ToImmutableArray();
}