using GeneralVokiCreationService.Application.draft_vokis.queries;
using VokiCreationServicesLib.Api.contracts;
using VokiCreationServicesLib.Api.contracts.voki_publishing;

namespace GeneralVokiCreationService.Api;

public record VokiPublishingDataResponse(
    string PrimaryAuthorId,
    string[] CoAuthorIds,
    string[] UserIdsToBecomeManagers,
    VokiPublishingIssueResponse[] Issues
) : ICreatableResponse<GetVokiPublishingDataQueryResult>
{
    public static ICreatableResponse<GetVokiPublishingDataQueryResult> Create(GetVokiPublishingDataQueryResult res) =>
        new VokiPublishingDataResponse(
            res.PrimaryAuthorId.ToString(),
            CoAuthorIds: res.CoAuthors.ToImmutableHashSet().Select(i => i.ToString()).ToArray(),
            UserIdsToBecomeManagers: res.UserIdsToBecomeManagers.Select(i => i.ToString()).ToArray(),
            res.Issues.Select(VokiPublishingIssueResponse.Create).ToArray()
        );
}