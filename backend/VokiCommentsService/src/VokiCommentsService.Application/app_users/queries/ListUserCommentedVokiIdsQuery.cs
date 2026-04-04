using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.user_ctx;
using VokiCommentsService.Application.common.repositories;

namespace VokiCommentsService.Application.app_users.queries;

public sealed record ListUserCommentedVokiIdsQuery() :
    IQuery<VokiIdWithLastCommentedDateDto[]>,
    IWithAuthCheckStep;

internal sealed class ListUserCommentedVokiIdsQueryHandler :
    IQueryHandler<ListUserCommentedVokiIdsQuery, VokiIdWithLastCommentedDateDto[]>
{
    private readonly IUserCtxProvider _userCtxProvider;
    private readonly ICommentsRepository _commentsRepository;

    public ListUserCommentedVokiIdsQueryHandler(IUserCtxProvider userCtxProvider, ICommentsRepository commentsRepository) {
        _userCtxProvider = userCtxProvider;
        _commentsRepository = commentsRepository;
    }

    public async Task<ErrOr<VokiIdWithLastCommentedDateDto[]>> Handle(
        ListUserCommentedVokiIdsQuery query, CancellationToken ct
    ) {
        return await _commentsRepository.OrderedIdsOfVokiCommentedByUser(query.UserCtx(_userCtxProvider), ct);
    }
}