using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Api.contracts;

public record class RatingsWithAverageResponse(
    VokiRatingDataResponse[] Ratings,
    double AverageRating
) : ICreatableResponse<VokiRating[]>
{
    public static ICreatableResponse<VokiRating[]> Create(VokiRating[] ratings) => new RatingsWithAverageResponse(
        ratings.Select(VokiRatingDataResponse.FromRating).ToArray(),
        CalculateAverageRating(ratings)
    );

    private static double CalculateAverageRating(VokiRating[] ratings) =>
        ratings.Length == 0 
            ? 0
            : (double)ratings.Sum(r => r.CurrentValue.Value) / ratings.Length;
}