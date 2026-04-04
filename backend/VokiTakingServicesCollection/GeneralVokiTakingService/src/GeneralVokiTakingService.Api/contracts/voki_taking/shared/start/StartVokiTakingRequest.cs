namespace GeneralVokiTakingService.Api.contracts.voki_taking.shared.start;

public class StartVokiTakingRequest : IRequestWithValidationNeeded
{
    public bool TerminateExistingUnfinishedSession { get; init; } = false;

    public ErrOrNothing Validate() => ErrOrNothing.Nothing;
}