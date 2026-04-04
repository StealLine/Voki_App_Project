namespace GeneralVokiCreationService.Application.draft_vokis.queries.questions;

public sealed record GetVokiQuestionsOverviewQuery(VokiId VokiId) :
    IQuery<GetVokiQuestionsOverviewQueryResult>,
    IWithAuthCheckStep;

internal sealed class GetVokiQuestionsOverviewQueryHandler
    : IQueryHandler<GetVokiQuestionsOverviewQuery, GetVokiQuestionsOverviewQueryResult>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public GetVokiQuestionsOverviewQueryHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        IUserCtxProvider userCtxProvider
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<GetVokiQuestionsOverviewQueryResult>> Handle(GetVokiQuestionsOverviewQuery query,
        CancellationToken ct) {
        DraftGeneralVoki? voki = await _draftGeneralVokisRepository.GetWithQuestions(query.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki();
        }

        var aUserCtx = query.UserCtx(_userCtxProvider);
        if (!voki.HasUserAccess(aUserCtx)) {
            return ErrFactory.NoAccess("User don't have access to this Voki");
        }

        ErrOr<ImmutableArray<VokiQuestion>> questions = voki.GetQuestions(aUserCtx);
        if (questions.IsErr(out var err)) {
            return err;
        }

        return new GetVokiQuestionsOverviewQueryResult(
            questions.AsSuccess(),
            voki.TakingProcessSettings
        );
    }
}

public record GetVokiQuestionsOverviewQueryResult(
    ImmutableArray<VokiQuestion> Questions,
    VokiTakingProcessSettings Settings
);