using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel;
using SharedKernel.user_ctx;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.app_user_aggregate;
using VokiRatingsService.Domain.common;
using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Application.voki_ratings.commands;

public sealed record RateVokiCommand(
    VokiId VokiId,
    RatingValue NewRating
) : ICommand<VokiRating>,
    IWithAuthCheckStep;

internal sealed class RateVokiCommandHandler : ICommandHandler<RateVokiCommand, VokiRating>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IRatingsRepository _ratingsRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserCtxProvider _userCtxProvider;

    public RateVokiCommandHandler(
        IAppUsersRepository appUsersRepository,
        IRatingsRepository ratingsRepository,
        IDateTimeProvider dateTimeProvider,
        IUserCtxProvider userCtxProvider
    ) {
        _appUsersRepository = appUsersRepository;
        _ratingsRepository = ratingsRepository;
        _dateTimeProvider = dateTimeProvider;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<VokiRating>> Handle(RateVokiCommand command, CancellationToken ct) {
        var aUserCtx = command.UserCtx(_userCtxProvider);
        AppUser user = (await _appUsersRepository.GetCurrentForUpdate(aUserCtx, ct))!;
        if (!user.HasTaken(command.VokiId)) {
            return ErrFactory.NoAccess(
                "You cannot rate this Voki because you have not taken it yet",
                "To rate voki you need to take it"
            );
        }


        VokiRating? rating = await _ratingsRepository.GetUserRatingForVokiForUpdate(aUserCtx, command.VokiId, ct);


        if (rating is null) {
            rating = VokiRating.CreateNew(aUserCtx.UserId, command.VokiId, command.NewRating, _dateTimeProvider.UtcNow);
            await _ratingsRepository.Add(rating, ct);
        }
        else {
            ErrOrNothing res = rating.Update(command.NewRating, _dateTimeProvider.UtcNow);
            if (res.IsErr(out var updateRatingErr)) {
                return updateRatingErr;
            }

            await _ratingsRepository.Update(rating, ct);
        }

        return rating;
    }
}