using SharedKernel.common.vokis;

namespace VokiCreationServicesLib.Api.contracts.update_requests;

public class UpdateVokiNameRequest : IRequestWithValidationNeeded
{
    public string NewName { get; init; }
    public ErrOrNothing Validate() => VokiName.CheckForErr(NewName);

    public VokiName ParsedVokiName => new(NewName);
}