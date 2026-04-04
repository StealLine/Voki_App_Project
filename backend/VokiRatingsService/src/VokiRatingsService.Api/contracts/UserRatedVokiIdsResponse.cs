using VokiRatingsService.Application.common.repositories;

namespace VokiRatingsService.Api.contracts;

public record class UserRatedVokiIdsResponse(
    Dictionary<string, UserRatedVokiIdsResponse.RatingBriefDataResponse> VokiIdToLastRatingData
) : ICreatableResponse<VokiIdWithCurrentRatingDto[]>
{
    public static ICreatableResponse<VokiIdWithCurrentRatingDto[]> Create(VokiIdWithCurrentRatingDto[] vokis) =>
        new UserRatedVokiIdsResponse(vokis.ToDictionary(
            v => v.VokiId.ToString(),
            v => new RatingBriefDataResponse(v.Value.Value, v.DateTime))
        );

    public record RatingBriefDataResponse(ushort Value, DateTime DateTime);
}