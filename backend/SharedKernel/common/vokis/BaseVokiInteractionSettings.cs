using SharedKernel.common.vokis.general_vokis;
using SharedKernel.common.vokis.scoring_voki;
using SharedKernel.common.vokis.tier_list_voki;

namespace SharedKernel.common.vokis;

public abstract class BaseVokiInteractionSettings : ValueObject
{
    public abstract bool SignedInOnlyTaking { get; }

    public TResult Match<TResult>(
        Func<GeneralVokiInteractionSettings, TResult> onGeneral,
        Func<ScoringVokiInteractionSettings, TResult> onScoring,
        Func<TierListVokiInteractionSettings, TResult> onTierList
    ) => this switch
    {
        GeneralVokiInteractionSettings g => onGeneral(g),
        ScoringVokiInteractionSettings s => onScoring(s),
        TierListVokiInteractionSettings t => onTierList(t),
        _ => throw new InvalidOperationException($"Unknown interaction settings type: {GetType().Name}")
    };

    public override IEnumerable<object> GetEqualityComponents() => [SignedInOnlyTaking];
}