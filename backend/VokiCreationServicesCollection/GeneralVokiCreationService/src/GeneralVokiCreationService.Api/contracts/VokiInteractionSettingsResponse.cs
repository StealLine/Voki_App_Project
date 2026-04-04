using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiCreationService.Api.contracts;

public record VokiInteractionSettingsResponse(
    bool SignedInOnlyTaking,
    GeneralVokiResultsVisibility ResultsVisibility,
    bool ShowResultsDistribution
) : ICreatableResponse<GeneralVokiInteractionSettings>
{
    public static ICreatableResponse<GeneralVokiInteractionSettings> Create(GeneralVokiInteractionSettings settings) =>
        FromSettings(settings);

    public static VokiInteractionSettingsResponse FromSettings(GeneralVokiInteractionSettings settings) => new(
        SignedInOnlyTaking: settings.SignedInOnlyTaking,
        ResultsVisibility: settings.ResultsVisibility,
        ShowResultsDistribution: settings.ShowResultsDistribution
    );
}