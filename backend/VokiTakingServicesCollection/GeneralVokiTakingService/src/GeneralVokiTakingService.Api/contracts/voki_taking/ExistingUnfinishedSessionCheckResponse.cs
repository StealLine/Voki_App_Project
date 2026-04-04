using GeneralVokiTakingService.Application.dtos;
using GeneralVokiTakingService.Application.general_vokis.queries;

namespace GeneralVokiTakingService.Api.contracts.voki_taking;

public record ExistingUnfinishedSessionCheckResponse(
    bool DoesUnfinishedSessionExist,
    ExistingUnfinishedSessionCheckResponse.VokiTakingSessionDataResponse? SessionData
) : ICreatableResponse<CheckIfUserHasUnfinishedSessionForVokiQueryResult>
{
    public record VokiTakingSessionDataResponse(
        string VokiId,
        string SessionId,
        DateTime StartedAt,
        ushort QuestionsWithSavedAnswersCount,
        ushort TotalQuestionsCount
    ) : IExistingUnfinishedVokiTakingSessionResponse
    {
        public static VokiTakingSessionDataResponse Create(SavedUnfinishedVokiTakingSessionDto dto) => new(
            dto.VokiId.ToString(),
            dto.SessionId.ToString(),
            dto.StartedAt,
            dto.QuestionsWithSavedAnswersCount,
            dto.TotalQuestionsCount
        );
    }

    public static ICreatableResponse<CheckIfUserHasUnfinishedSessionForVokiQueryResult> Create(
        CheckIfUserHasUnfinishedSessionForVokiQueryResult res
    ) => res.Match<ExistingUnfinishedSessionCheckResponse>(
        sessionExistsFunc: s => new(
            DoesUnfinishedSessionExist: true,
            SessionData: VokiTakingSessionDataResponse.Create(s)),
        noSessionFunc: () => new(false, null)
    );
}