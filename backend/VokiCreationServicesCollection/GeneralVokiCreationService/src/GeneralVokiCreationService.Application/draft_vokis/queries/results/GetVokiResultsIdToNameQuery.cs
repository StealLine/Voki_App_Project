using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;
using SharedKernel;

namespace GeneralVokiCreationService.Application.draft_vokis.queries.results;

public sealed record GetVokiResultsIdToNameQuery(VokiId VokiId) :
    IQuery<ImmutableDictionary<GeneralVokiResultId, VokiResultName>>,
    IWithAuthCheckStep;

internal sealed class GetVokiResultsIdToNameQueryHandler :
    IQueryHandler<GetVokiResultsIdToNameQuery, ImmutableDictionary<GeneralVokiResultId, VokiResultName>>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public GetVokiResultsIdToNameQueryHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        IUserCtxProvider userCtxProvider
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<ImmutableDictionary<GeneralVokiResultId, VokiResultName>>> Handle(
        GetVokiResultsIdToNameQuery query, CancellationToken ct
    ) {
        DraftGeneralVoki? voki = await _draftGeneralVokisRepository.GetWithResults(query.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki();
        }

        return voki
            .GetResults(query.UserCtx(_userCtxProvider))
            .Bind<ImmutableDictionary<GeneralVokiResultId, VokiResultName>>(
                res => res.ToImmutableDictionary(
                    r => r.Id,
                    r => r.Name
                )
            );
    }
}
