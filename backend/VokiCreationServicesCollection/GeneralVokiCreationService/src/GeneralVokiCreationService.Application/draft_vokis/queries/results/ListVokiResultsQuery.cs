namespace GeneralVokiCreationService.Application.draft_vokis.queries.results;

public sealed record ListVokiResultsQuery(
    VokiId VokiId
) :
    IQuery<ImmutableArray<VokiResult>>,
    IWithAuthCheckStep;

internal sealed class ListVokiResultsQueryHandler : IQueryHandler<ListVokiResultsQuery, ImmutableArray<VokiResult>>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public ListVokiResultsQueryHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        IUserCtxProvider userCtxProvider
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<ImmutableArray<VokiResult>>> Handle(ListVokiResultsQuery query, CancellationToken ct) {
        DraftGeneralVoki? voki = await _draftGeneralVokisRepository.GetWithResults(query.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki();
        }

        var aUserCtx = query.UserCtx(_userCtxProvider);
        return voki.GetResults(aUserCtx);
    }
}