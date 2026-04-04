namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

public record class VokiTakingProcessSettings(
    bool ForceSequentialAnswering,
    bool ShuffleQuestions
)
{
    public static readonly VokiTakingProcessSettings Default = new(false, false);
}