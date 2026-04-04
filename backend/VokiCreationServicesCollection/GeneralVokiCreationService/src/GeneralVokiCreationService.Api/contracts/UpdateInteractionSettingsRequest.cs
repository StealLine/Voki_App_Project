using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiCreationService.Api.contracts;

public class UpdateInteractionSettingsRequest : IRequestWithValidationNeeded
{
    public bool SignedInOnlyTaking { init; get; }
    public GeneralVokiResultsVisibility ResultsVisibility { init; get; }
    public bool ShowResultsDistribution { init; get; }

    public ErrOrNothing Validate() {
        var res = GeneralVokiInteractionSettings.Create(
            signedInOnlyTaking: SignedInOnlyTaking,
            resultsVisibility: ResultsVisibility,
            showResultsDistribution: ShowResultsDistribution
        );
        if (res.IsErr(out var err)) {
            return err;
        }

        ParsedSettings = res.AsSuccess();
        return ErrOrNothing.Nothing;
    }

    public GeneralVokiInteractionSettings ParsedSettings { get; private set; }
}