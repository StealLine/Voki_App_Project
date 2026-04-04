namespace SharedKernel.common.vokis.scoring_voki;

public sealed class ScoringVokiInteractionSettings : BaseVokiInteractionSettings
{
    public override bool SignedInOnlyTaking { get; }
    public override IEnumerable<object> GetEqualityComponents() => [SignedInOnlyTaking];

    public ScoringVokiInteractionSettings(bool signedInOnlyTaking)
    {
        SignedInOnlyTaking = signedInOnlyTaking;
    }
}