using CoreVokiCreationService.Domain.draft_voki_aggregate;
using SharedKernel.common.vokis;

namespace CoreVokiCreationService.Api.contracts.init_new_voki;

public record NewVokiInitializedResponse(
    string Id,
    VokiType Type,
    string Name
) : ICreatableResponse<DraftVoki>
{
    public static ICreatableResponse<DraftVoki> Create(DraftVoki voki) => new NewVokiInitializedResponse(
        voki.Id.ToString(),
        voki.Type,
        voki.Name.ToString()
    );
}