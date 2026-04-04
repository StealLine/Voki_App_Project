using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;

namespace GeneralVokiCreationService.Api.contracts.results;

public class UpdateVokiResultRequest : IRequestWithValidationNeeded
{
    public string NewName { get; init; }
    public string NewText { get; init; }
    public string? NewImage { get; init; }

    public ErrOrNothing Validate() =>
        VokiResultName.CheckForErr(NewName).WithNextIfErr(
            VokiResultText.CheckForErr(NewText));


    public VokiResultName ParsedName => VokiResultName.Create(NewName).AsSuccess();
    public VokiResultText ParsedText => VokiResultText.Create(NewText).AsSuccess();
}