using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

namespace GeneralVokiCreationService.Api.contracts.results;

public record class VokiResultDataResponse(
    string Id,
    string Name,
    string Text,
    string? Image
) : ICreatableResponse<VokiResult>
{
    public static VokiResultDataResponse FromResult(VokiResult result) => new(
        result.Id.ToString(),
        result.Name.ToString(),
        result.Text.ToString(),
        result.Image?.ToString() ?? null
    );

    public static ICreatableResponse<VokiResult> Create(VokiResult result) => FromResult(result);
}