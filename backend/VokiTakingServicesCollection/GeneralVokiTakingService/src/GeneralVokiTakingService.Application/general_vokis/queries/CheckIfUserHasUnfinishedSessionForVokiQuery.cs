using ApplicationShared;
using GeneralVokiTakingService.Application.common.repositories.taking_sessions;
using GeneralVokiTakingService.Application.dtos;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

namespace GeneralVokiTakingService.Application.general_vokis.queries;

public record CheckIfUserHasUnfinishedSessionForVokiQuery(
    VokiId VokiId
) : IQuery<CheckIfUserHasUnfinishedSessionForVokiQueryResult>;

internal sealed class CheckIfUserHasUnfinishedSessionForVokiQueryHandler :
    IQueryHandler<CheckIfUserHasUnfinishedSessionForVokiQuery, CheckIfUserHasUnfinishedSessionForVokiQueryResult>
{
    private readonly IBaseTakingSessionsRepository _sessionsRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public CheckIfUserHasUnfinishedSessionForVokiQueryHandler(
        IBaseTakingSessionsRepository sessionsRepository,
        IUserCtxProvider userCtxProvider
    ) {
        _sessionsRepository = sessionsRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<CheckIfUserHasUnfinishedSessionForVokiQueryResult>> Handle(
        CheckIfUserHasUnfinishedSessionForVokiQuery query, CancellationToken ct
    ) {
        if (!_userCtxProvider.Current.IsAuthenticated(out var aUserCtx)) {
            return CheckIfUserHasUnfinishedSessionForVokiQueryResult.CreateNoSession();
        }

        BaseVokiTakingSession? session = await _sessionsRepository.GetForVokiAndUser(query.VokiId, aUserCtx, ct);

        if (session is null) {
            return CheckIfUserHasUnfinishedSessionForVokiQueryResult.CreateNoSession();
        }

        return CheckIfUserHasUnfinishedSessionForVokiQueryResult.CreateWithSessionExists(
            SavedUnfinishedVokiTakingSessionDto.Create(session)
        );
    }
}

public sealed class CheckIfUserHasUnfinishedSessionForVokiQueryResult
{
    private CheckIfUserHasUnfinishedSessionForVokiQueryResult(SavedUnfinishedVokiTakingSessionDto? unfinishedSessionDto) {
        UnfinishedSessionDto = unfinishedSessionDto;
    }

    private SavedUnfinishedVokiTakingSessionDto? UnfinishedSessionDto { get; }

    public T Match<T>(Func<SavedUnfinishedVokiTakingSessionDto, T> sessionExistsFunc, Func<T> noSessionFunc) =>
        UnfinishedSessionDto is null ? noSessionFunc() : sessionExistsFunc(UnfinishedSessionDto);

    public static CheckIfUserHasUnfinishedSessionForVokiQueryResult CreateNoSession() => new(null);

    public static CheckIfUserHasUnfinishedSessionForVokiQueryResult CreateWithSessionExists(
        SavedUnfinishedVokiTakingSessionDto session
    ) => new(session);
}