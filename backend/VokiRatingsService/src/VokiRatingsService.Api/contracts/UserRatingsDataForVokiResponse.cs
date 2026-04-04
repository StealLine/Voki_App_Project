using VokiRatingsService.Application.voki_ratings.queries;

namespace VokiRatingsService.Api.contracts;

public record class UserRatingsDataForVokiResponse(
    bool UserHasTaken,
    RatingsWithAverageResponse RatingsWithAverage
) : ICreatableResponse<UserRatingsDataForVokiQueryResult>
{
    public static ICreatableResponse<UserRatingsDataForVokiQueryResult> Create(
        UserRatingsDataForVokiQueryResult success
    ) => new UserRatingsDataForVokiResponse(
        success.UserHasTaken,
        (RatingsWithAverageResponse)RatingsWithAverageResponse.Create(success.Ratings)
    );
}