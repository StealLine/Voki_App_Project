using ApplicationShared;
using SharedKernel.user_ctx;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.app_user_aggregate;
using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Application.voki_ratings.queries;

public sealed record UserRatingsDataForVokiQuery(
    VokiId VokiId
) : IQuery<UserRatingsDataForVokiQueryResult>;

internal sealed class UserRatingsDataForVokiQueryHandler :
    IQueryHandler<UserRatingsDataForVokiQuery, UserRatingsDataForVokiQueryResult>
{
    private readonly IUserCtxProvider _userCtxProvider;
    private readonly IRatingsRepository _ratingsRepository;
    private readonly IAppUsersRepository _appUsersRepository;

    public UserRatingsDataForVokiQueryHandler(
        IUserCtxProvider userCtxProvider,
        IRatingsRepository ratingsRepository,
        IAppUsersRepository appUsersRepository
    ) {
        _userCtxProvider = userCtxProvider;
        _ratingsRepository = ratingsRepository;
        _appUsersRepository = appUsersRepository;
    }

    public Task<ErrOr<UserRatingsDataForVokiQueryResult>> Handle(
        UserRatingsDataForVokiQuery query, CancellationToken ct
    ) =>
        _userCtxProvider.Current.Match(
            authenticatedFunc: aUserCtx => GetRatingsDataForAuthenticatedUser(aUserCtx, query.VokiId, ct),
            unauthenticatedFunc: _ => GetRatingsDataForNotAuthenticatedUser(query.VokiId, ct)
        );

    private async Task<ErrOr<UserRatingsDataForVokiQueryResult>> GetRatingsDataForAuthenticatedUser(
        AuthenticatedUserCtx aUserCtx, VokiId vokiId, CancellationToken ct
    ) {
        VokiRating[] ratings = (await _ratingsRepository.ListRatingsForVoki(vokiId, ct));
        VokiRating? userRating = ratings.SingleOrDefault(r => r.UserId == aUserCtx.UserId);
        bool hasTaken = false;

        if (userRating is not null) {
            hasTaken = true;
        }
        else {
            AppUser user = (await _appUsersRepository.GetCurrent(aUserCtx, ct))!;
            hasTaken = user.HasTaken(vokiId);
        }


        return new UserRatingsDataForVokiQueryResult(hasTaken, ratings);
    }

    private async Task<ErrOr<UserRatingsDataForVokiQueryResult>> GetRatingsDataForNotAuthenticatedUser(
        VokiId vokiId, CancellationToken ct
    ) => new UserRatingsDataForVokiQueryResult(
        false,
        await _ratingsRepository.ListRatingsForVoki(vokiId, ct)
    );
}

public sealed record UserRatingsDataForVokiQueryResult(bool UserHasTaken, VokiRating[] Ratings);