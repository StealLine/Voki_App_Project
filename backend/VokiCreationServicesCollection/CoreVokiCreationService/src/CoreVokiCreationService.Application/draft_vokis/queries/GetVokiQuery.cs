using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Application.draft_vokis.queries;

public sealed record GetVokiQuery(VokiId VokiId) :
    IQuery<DraftVoki>,
    IWithAuthCheckStep;

internal sealed class GetVokiQueryHandler : IQueryHandler<GetVokiQuery, DraftVoki>
{
    private readonly IDraftVokiRepository _draftVokiRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public GetVokiQueryHandler(IDraftVokiRepository draftVokiRepository, IUserCtxProvider userCtxProvider) {
        _draftVokiRepository = draftVokiRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<DraftVoki>> Handle(GetVokiQuery query, CancellationToken ct) {
        DraftVoki? voki = await _draftVokiRepository.GetById(query.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki(
                "Requested Voki not found", $"Voki with id {query.VokiId} does not exist"
            );
        }

        if (voki.DoesUserHaveAccess(query.UserCtx(_userCtxProvider))) {
            return voki;
        }

        return ErrFactory.NoAccess("You do not have access to this Voki");
    }
}