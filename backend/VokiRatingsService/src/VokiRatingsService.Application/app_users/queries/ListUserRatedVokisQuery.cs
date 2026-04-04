using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.user_ctx;
using VokiRatingsService.Application.common.repositories;

namespace VokiRatingsService.Application.app_users.queries;

public sealed record ListUserRatedVokisQuery() :
    IQuery<VokiIdWithCurrentRatingDto[]>,
    IWithAuthCheckStep
{
    public Err UnauthenticatedErr => ErrFactory.AuthRequired(
        "To see your rated Vokis you need to log into your account"
    );
}

internal sealed class ListUserRatedVokisQueryHandler :
    IQueryHandler<ListUserRatedVokisQuery, VokiIdWithCurrentRatingDto[]>
{
    private readonly IUserCtxProvider _userCtxProvider;
    private readonly IRatingsRepository _ratingsRepository;

    public ListUserRatedVokisQueryHandler(IUserCtxProvider userCtxProvider, IRatingsRepository ratingsRepository) {
        _userCtxProvider = userCtxProvider;
        _ratingsRepository = ratingsRepository;
    }


    public async Task<ErrOr<VokiIdWithCurrentRatingDto[]>> Handle(ListUserRatedVokisQuery query, CancellationToken ct) {
        return await _ratingsRepository.ListIdsOfVokiRatedByUser(query.UserCtx(_userCtxProvider), ct);
    }
}