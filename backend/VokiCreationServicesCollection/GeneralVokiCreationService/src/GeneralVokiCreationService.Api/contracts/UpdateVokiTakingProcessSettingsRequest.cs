using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

namespace GeneralVokiCreationService.Api.contracts;

public class UpdateVokiTakingProcessSettingsRequest : IRequestWithValidationNeeded
{
    public bool ForceSequentialAnswering { get; init; }
    public bool ShuffleQuestions { get; init; }
    public ErrOrNothing Validate() => ErrOrNothing.Nothing;

    public VokiTakingProcessSettings ParsedSettings => new (
        ForceSequentialAnswering: ForceSequentialAnswering,
        ShuffleQuestions: ShuffleQuestions
    );
}