using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace VokiCreationServicesLib.Api.contracts;

public record class VokiDetailsResponse(
    string Description,
    Language Language,
    bool HasMatureContent
) : ICreatableResponse<VokiDetails>
{
    public static ICreatableResponse<VokiDetails> Create(VokiDetails details) => FromDetails(details);
    public static VokiDetailsResponse FromDetails(VokiDetails details) => new (
        details.Description.ToString(),
        details.Language,
        details.HasMatureContent
    );
}