using VokiCreationServicesLib.Application;

namespace VokiCreationServicesLib.Api.contracts.voki_publishing;

public record class VokiSuccessfullyPublishedResponse(
    string Id,
    string Cover,
    string Name
) : ICreatableResponse<VokiSuccessfullyPublishedResult>
{
    public static ICreatableResponse<VokiSuccessfullyPublishedResult> Create(VokiSuccessfullyPublishedResult result) =>
        new VokiSuccessfullyPublishedResponse(
            result.Id.ToString(),
            result.Cover.ToString(),
            result.Name.ToString()
        );
}