using ApplicationShared;
using GeneralVokiTakingService.Application.common.repositories;
using GeneralVokiTakingService.Domain.app_user_aggregate;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using SharedKernel.common.vokis;
using SharedKernel.common.vokis.general_vokis;
using SharedKernel.user_ctx;

namespace GeneralVokiTakingService.Application.general_vokis.queries;

public sealed record ViewVokiResultQuery(VokiId VokiId, GeneralVokiResultId ResultId) : IQuery<ViewVokiResultQueryResult>;

internal sealed class ViewVokiResultQueryHandler : IQueryHandler<ViewVokiResultQuery, ViewVokiResultQueryResult>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IGeneralVokisRepository _generalVokisRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public ViewVokiResultQueryHandler(
        IGeneralVokisRepository generalVokisRepository, IUserCtxProvider userCtxProvider, IAppUsersRepository appUsersRepository
    ) {
        _generalVokisRepository = generalVokisRepository;
        _userCtxProvider = userCtxProvider;
        _appUsersRepository = appUsersRepository;
    }


    public async Task<ErrOr<ViewVokiResultQueryResult>> Handle(ViewVokiResultQuery query, CancellationToken ct) {
        GeneralVoki? voki = await _generalVokisRepository.GetWithResultsById(query.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.GeneralVoki();
        }

        return await voki.InteractionSettings.ResultsVisibility.Match(
            anyone: () => ResultToViewByAnyOne(voki, query.ResultId, ct),
            afterTaking: () => ResultToViewByUserAfterTaking(voki, query.ResultId, ct),
            onlyReceived: () => OnlyReceivedResultToViewByUser(voki, query.ResultId, ct)
        );
    }

    private async Task<ErrOr<ViewVokiResultQueryResult>> ResultToViewByAnyOne(
        GeneralVoki voki, GeneralVokiResultId resultId, CancellationToken ct
    ) {
        ErrOr<VokiResult> resultOrErr = voki.GetResultToViewByAnyOne(resultId);
        if (resultOrErr.IsErr(out var err)) {
            return err;
        }

        VokiResult r = resultOrErr.AsSuccess();
        bool hasUserTakenThisVoki = false;
        if (_userCtxProvider.Current.IsAuthenticated(out var aUserCtx)) {
            AppUser? user = await _appUsersRepository.GetCurrent(aUserCtx, ct);
            if (user is null) {
                return ErrFactory.NotFound.User("Cannot find user account. Please log out and login again");
            }

            hasUserTakenThisVoki = voki.UserHasTakenThisVoki(user.ReceivedResultIds);
        }

        return new ViewVokiResultQueryResult(
            r, voki.InteractionSettings.ResultsVisibility, voki.Name,
            voki.ResultsCount, hasUserTakenThisVoki
        );
    }

    private Task<ErrOr<ViewVokiResultQueryResult>> ResultToViewByUserAfterTaking(
        GeneralVoki voki,
        GeneralVokiResultId resultId,
        CancellationToken ct
    ) => _userCtxProvider.Current.Match<Task<ErrOr<ViewVokiResultQueryResult>>>(
        authenticatedFunc: async aUserCtx => {
            AppUser? user = await _appUsersRepository.GetCurrent(aUserCtx, ct);
            if (user is null) {
                return ErrFactory.NotFound.User("Cannot find user account. Please log out and login again");
            }

            return voki.GetResultToViewByUserAfterTaking(resultId, user.ReceivedResultIds).Bind<ViewVokiResultQueryResult>(
                r => new ViewVokiResultQueryResult(
                    r, voki.InteractionSettings.ResultsVisibility, voki.Name,
                    voki.ResultsCount, voki.UserHasTakenThisVoki(user.ReceivedResultIds)
                )
            );
        }, _ => Task.FromResult<ErrOr<ViewVokiResultQueryResult>>(
            ErrFactory.NoAccess("To see this voki results you need to login and take this voki at least once")
        )
    );


    private Task<ErrOr<ViewVokiResultQueryResult>> OnlyReceivedResultToViewByUser(
        GeneralVoki voki,
        GeneralVokiResultId resultId,
        CancellationToken ct
    ) => _userCtxProvider.Current.Match<Task<ErrOr<ViewVokiResultQueryResult>>>(
        authenticatedFunc: async aUserCtx => {
            AppUser? user = await _appUsersRepository.GetCurrent(aUserCtx, ct);
            if (user is null) {
                return ErrFactory.NotFound.User("Cannot find user account. Please log out and login again");
            }

            return voki.GetOnlyReceivedResultToViewByUser(resultId, user.ReceivedResultIds).Bind<ViewVokiResultQueryResult>(
                r => new ViewVokiResultQueryResult(
                    r, voki.InteractionSettings.ResultsVisibility, voki.Name,
                    voki.ResultsCount, voki.UserHasTakenThisVoki(user.ReceivedResultIds)
                )
            );
        }, _ => Task.FromResult<ErrOr<ViewVokiResultQueryResult>>(
            ErrFactory.NoAccess("To see this voki results you need to login and take this voki at least once")
        )
    );
}

public sealed record ViewVokiResultQueryResult(
    VokiResult Result,
    GeneralVokiResultsVisibility ResultsVisibility,
    VokiName VokiName,
    uint TotalResultsCount,
    bool HasUserTakenThisVoki
);