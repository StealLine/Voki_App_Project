using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

namespace GeneralVokiCreationService.Application.draft_vokis.queries;

public sealed record GetVokiQuery(VokiId VokiId) :
    IQuery<DraftGeneralVoki>,
    IWithAuthCheckStep;

internal sealed class GetVokiQueryHandler : IQueryHandler<GetVokiQuery, DraftGeneralVoki>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly IUserCtxProvider _userCtxProvider;


    public GetVokiQueryHandler(IDraftGeneralVokisRepository draftGeneralVokisRepository, IUserCtxProvider userCtxProvider) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<DraftGeneralVoki>> Handle(GetVokiQuery query, CancellationToken ct) {
        var voki = await _draftGeneralVokisRepository.GetById(query.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki("There is no such draft general voki");
        }

        if (!voki.HasUserAccess(query.UserCtx(_userCtxProvider))) {
            return ErrFactory.NotFound.Voki("You don't have access to this Voki");
        }

        return voki;
    }
}