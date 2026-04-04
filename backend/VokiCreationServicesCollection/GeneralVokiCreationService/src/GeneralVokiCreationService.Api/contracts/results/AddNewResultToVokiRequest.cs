using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;

namespace GeneralVokiCreationService.Api.contracts.results;

public class AddNewResultToVokiRequest : IRequestWithValidationNeeded
{
    public string ResultName { get; init; }
    public ErrOrNothing Validate() => VokiResultName.CheckForErr(ResultName);
    public VokiResultName ParsedName => VokiResultName.Create(ResultName).AsSuccess();
}