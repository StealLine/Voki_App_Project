using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace VokiCreationServicesLib.Api.contracts.update_requests;

public class UpdateVokiDetailsRequest : IRequestWithValidationNeeded
{
    public string NewDescription { get; init; }
    public Language NewLanguage { get; init; }
    public bool NewHasMatureContent { get; init; }
    public ErrOrNothing Validate() => VokiDescription.CheckForErr(NewDescription);

    public VokiDetails ParsedDetails => new(
        VokiDescription.Create(NewDescription).AsSuccess(),
        NewHasMatureContent,
        NewLanguage
    );
}