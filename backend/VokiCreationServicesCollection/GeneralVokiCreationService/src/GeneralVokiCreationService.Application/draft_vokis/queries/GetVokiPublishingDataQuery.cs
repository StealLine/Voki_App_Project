using VokiCreationServicesLib.Domain.draft_voki_aggregate;
using VokiCreationServicesLib.Domain.draft_voki_aggregate.publishing;

namespace GeneralVokiCreationService.Application.draft_vokis.queries;

public sealed record GetVokiPublishingDataQuery(VokiId VokiId) :
    IQuery<GetVokiPublishingDataQueryResult>,
    IWithAuthCheckStep;

internal sealed class GetVokiPublishingDataQueryHandler :
    IQueryHandler<GetVokiPublishingDataQuery, GetVokiPublishingDataQueryResult>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public GetVokiPublishingDataQueryHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        IUserCtxProvider userCtxProvider
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<GetVokiPublishingDataQueryResult>> Handle(GetVokiPublishingDataQuery query, CancellationToken ct) {
        DraftGeneralVoki? voki = await _draftGeneralVokisRepository.GetWithQuestionsAndResults(query.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki();
        }

        var issuesOrErr = voki.GatherAllPublishingIssues(query.UserCtx(_userCtxProvider));
        if (issuesOrErr.IsErr(out var err)) {
            return err;
        }

        return new GetVokiPublishingDataQueryResult(
            voki.PrimaryAuthorId,
            voki.CoAuthors,
            voki.UserIdsToBecomeManagers,
            issuesOrErr.AsSuccess()
        );
    }
}

public record GetVokiPublishingDataQueryResult(
    AppUserId PrimaryAuthorId,
    VokiCoAuthorIdsSet CoAuthors,
    ImmutableHashSet<AppUserId> UserIdsToBecomeManagers,
    ImmutableArray<VokiPublishingIssue> Issues
);