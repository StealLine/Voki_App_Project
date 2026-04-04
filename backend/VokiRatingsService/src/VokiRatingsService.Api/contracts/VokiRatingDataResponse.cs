using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Api.contracts;

public record VokiRatingDataResponse(
    string UserId,
    ushort Value,
    DateTime DateTime
) : ICreatableResponse<VokiRating>
{
    public static VokiRatingDataResponse FromRating(VokiRating rating) => new(
        rating.UserId.ToString(),
        rating.CurrentValue.Value,
        rating.LastUpdated
    );

    public static ICreatableResponse<VokiRating> Create(VokiRating rating) => FromRating(rating);
}