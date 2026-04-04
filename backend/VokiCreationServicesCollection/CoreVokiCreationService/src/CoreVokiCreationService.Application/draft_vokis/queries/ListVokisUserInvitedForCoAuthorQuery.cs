using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate;
using SharedKernel.user_ctx;

namespace CoreVokiCreationService.Application.draft_vokis.queries;

public sealed record ListVokisUserInvitedForCoAuthorQuery() :
    IQuery<DraftVoki[]>,
    IWithAuthCheckStep;

internal sealed class ViewVokiAsInvitedForCoAuthorQueryHandler
    : IQueryHandler<ListVokisUserInvitedForCoAuthorQuery, DraftVoki[]>
{
    private readonly IDraftVokiRepository _draftVokiRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public ViewVokiAsInvitedForCoAuthorQueryHandler(IDraftVokiRepository draftVokiRepository,
        IUserCtxProvider userCtxProvider) {
        _draftVokiRepository = draftVokiRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<DraftVoki[]>> Handle(ListVokisUserInvitedForCoAuthorQuery query, CancellationToken ct) =>
        await _draftVokiRepository.ListVokisWithCurrentUserAsInvitedForCoAuthor(
            query.UserCtx(_userCtxProvider), ct
        );
}