using VokisCatalogService.Domain.app_user_aggregate;

namespace VokisCatalogService.Api.contracts;

public record class UserTakenVokisResponse(
    Dictionary<string, UserTakenVokisResponse.VokiTakenBriefDataResponse> Vokis
) : ICreatableResponse<UserTakenVokisList>
{
    public static ICreatableResponse<UserTakenVokisList> Create(UserTakenVokisList l) =>
        new UserTakenVokisResponse(l.Value.ToDictionary(
            kvp => kvp.Key.ToString(),
            kvp => new VokiTakenBriefDataResponse(kvp.Value.TimesTaken, kvp.Value.LastTimeTaken)
        ));

    public record VokiTakenBriefDataResponse(uint TimesTaken, DateTime LastTimeTaken);
}