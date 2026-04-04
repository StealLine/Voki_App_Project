using System.Runtime.CompilerServices;
using GeneralVokiTakingService.Application.general_vokis.commands;

namespace GeneralVokiTakingService.Api.contracts.voki_taking.shared.start;

public abstract record StartVokiTakingResponse(
    string VokiId,
    string SessionId,
    DateTime StartedAt,
    ushort TotalQuestionsCount
) : ICreatableResponse<IStartVokiTakingCommandResult>
{
    public abstract bool NewSessionStarted { get; }

    public static ICreatableResponse<IStartVokiTakingCommandResult> Create(IStartVokiTakingCommandResult res) => res switch {
        SuccessStartVokiTakingCommandResult success =>
            VokiTakingSuccessfullyStartedResponse.Create(success),
        StartVokiTakingCommandUnfinishedSessionExistsResult unfinished =>
            StartVokiTakingVokiTakingSessionExistsResponse.Create(unfinished),
        _ => throw new SwitchExpressionException(res),
    };

    public sealed record VokiTakingSuccessfullyStartedResponse(
        string VokiId,
        string VokiName,
        bool IsWithForceSequentialAnswering,
        GeneralVokiTakingResponseQuestionData[] Questions,
        string SessionId,
        DateTime StartedAt,
        ushort TotalQuestionsCount
    ) : StartVokiTakingResponse(VokiId, SessionId, StartedAt, TotalQuestionsCount)
    {
        public override bool NewSessionStarted => true;

        public static VokiTakingSuccessfullyStartedResponse Create(SuccessStartVokiTakingCommandResult res) => new(
            res.SessionData.VokiId.ToString(),
            res.SessionData.VokiName.ToString(),
            res.SessionData.IsWithForceSequentialAnswering,
            res.SessionData.QuestionsToShow.Select(GeneralVokiTakingResponseQuestionData.FromQuestion).ToArray(),
            res.SessionData.SessionId.ToString(),
            res.SessionData.StartedAt,
            res.SessionData.TotalQuestionsCount
        );
    }

    public sealed record StartVokiTakingVokiTakingSessionExistsResponse(
        string VokiId,
        string SessionId,
        DateTime StartedAt,
        ushort QuestionsWithSavedAnswersCount,
        ushort TotalQuestionsCount
    ) : StartVokiTakingResponse(VokiId, SessionId, StartedAt, TotalQuestionsCount), IExistingUnfinishedVokiTakingSessionResponse
    {
        public override bool NewSessionStarted => false;

        public static StartVokiTakingVokiTakingSessionExistsResponse Create(
            StartVokiTakingCommandUnfinishedSessionExistsResult res
        ) => new(
            res.SessionData.VokiId.ToString(),
            res.SessionData.SessionId.ToString(),
            res.SessionData.StartedAt,
            res.SessionData.QuestionsWithSavedAnswersCount,
            res.SessionData.TotalQuestionsCount
        );
    }
}