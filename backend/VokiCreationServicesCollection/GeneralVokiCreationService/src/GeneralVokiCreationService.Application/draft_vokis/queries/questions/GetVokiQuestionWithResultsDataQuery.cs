using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;

namespace GeneralVokiCreationService.Application.draft_vokis.queries.questions;

public sealed record GetVokiQuestionWithResultsDataQuery(
    VokiId VokiId,
    GeneralVokiQuestionId QuestionId
) :
    IQuery<GetVokiQuestionWithAnswersAndResultsQueryResult>,
    IWithAuthCheckStep;

internal sealed class GetVokiQuestionWithResultsDataQueryHandler :
    IQueryHandler<GetVokiQuestionWithResultsDataQuery, GetVokiQuestionWithAnswersAndResultsQueryResult>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly IUserCtxProvider _userCtxProvider;


    public GetVokiQuestionWithResultsDataQueryHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        IUserCtxProvider userCtxProvider
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<GetVokiQuestionWithAnswersAndResultsQueryResult>> Handle(
        GetVokiQuestionWithResultsDataQuery query, CancellationToken ct
    ) {
        DraftGeneralVoki? voki = await _draftGeneralVokisRepository.GetWithQuestionsAndResults(query.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki();
        }

        var aUserCtx = query.UserCtx(_userCtxProvider);
        if (!voki.HasUserAccess(aUserCtx)) {
            return ErrFactory.NoAccess("User don't have access to this Voki");
        }

        ErrOr<VokiQuestion> question = voki.QuestionWithId(aUserCtx, query.QuestionId);
        if (question.IsErr(out var err)) {
            return err;
        }

        return voki.GetResults(aUserCtx).Bind<GetVokiQuestionWithAnswersAndResultsQueryResult>(res =>
            new GetVokiQuestionWithAnswersAndResultsQueryResult(
                question.AsSuccess(),
                res.ToDictionary(r => r.Id, r => r.Name)
            )
        );
    }
}

public record GetVokiQuestionWithAnswersAndResultsQueryResult(
    VokiQuestion Question,
    Dictionary<GeneralVokiResultId, VokiResultName> ResultsIdToName
);