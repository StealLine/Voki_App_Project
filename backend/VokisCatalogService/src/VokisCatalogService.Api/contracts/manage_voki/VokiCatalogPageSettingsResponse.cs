using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Api.contracts.manage_voki;

public record VokiCatalogPageSettingsResponse() : ICreatableResponse<Voki>
{
    public static ICreatableResponse<Voki> Create(Voki success) => new VokiCatalogPageSettingsResponse(
    );
}