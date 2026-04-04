namespace SharedKernel.common.vokis.tier_list_voki;

public sealed class TierListVokiInteractionSettings : BaseVokiInteractionSettings
{
    public override bool SignedInOnlyTaking { get; }
    public override IEnumerable<object> GetEqualityComponents() => [SignedInOnlyTaking];
    public TierListVokiInteractionSettings(bool signedInOnlyTaking)
    {
        SignedInOnlyTaking = signedInOnlyTaking;
    }
}