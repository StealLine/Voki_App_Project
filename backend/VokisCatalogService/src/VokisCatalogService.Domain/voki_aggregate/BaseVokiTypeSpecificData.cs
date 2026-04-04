using VokisCatalogService.Domain.voki_aggregate.type_specific_data;

namespace VokisCatalogService.Domain.voki_aggregate;

public abstract class BaseVokiTypeSpecificData : ValueObject
{
    public TResult Match<TResult>(
        Func<GeneralVokiTypeSpecificData, TResult> onGeneral,
        Func<ScoringVokiTypeSpecificData, TResult> onScoring,
        Func<TierListVokiTypeSpecificData, TResult> onTierList
    ) => this switch {
        GeneralVokiTypeSpecificData g => onGeneral(g),
        ScoringVokiTypeSpecificData s => onScoring(s),
        TierListVokiTypeSpecificData t => onTierList(t),
        _ => throw new InvalidOperationException($"Unknown type specific data type: {GetType().Name}")
    };
}