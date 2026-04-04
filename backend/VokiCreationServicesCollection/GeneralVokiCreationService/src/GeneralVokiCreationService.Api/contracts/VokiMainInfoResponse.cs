using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Api.contracts;

namespace GeneralVokiCreationService.Api.contracts;

public record class VokiMainInfoResponse(
    string Name,
    string Cover,
    string[] Tags,
    VokiDetailsResponse Details,
    VokiInteractionSettingsResponse InteractionSettings
) : ICreatableResponse<DraftGeneralVoki>
{
    public static ICreatableResponse<DraftGeneralVoki> Create(DraftGeneralVoki voki) => new VokiMainInfoResponse(
        voki.Name.ToString(),
        voki.Cover.ToString(),
        voki.Tags.Value.Select(t => t.ToString()).ToArray(),
        VokiDetailsResponse.FromDetails(voki.Details),
        VokiInteractionSettingsResponse.FromSettings(voki.InteractionSettings)
    );
}