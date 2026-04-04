using SharedKernel.common.vokis;

namespace CoreVokiCreationService.Api.contracts.init_new_voki;

public class InitializeNewVokiRequest : IRequestWithValidationNeeded
{
    public string NewVokiName { get; init; } = "";
    public VokiType VokiType { get; init; }
    public ErrOrNothing Validate() => VokiName.CheckForErr(NewVokiName);

    public VokiName ParseVokiName => VokiName.Create(NewVokiName).AsSuccess();
}