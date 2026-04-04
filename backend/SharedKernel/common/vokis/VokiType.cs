using System.ComponentModel;

namespace SharedKernel.common.vokis;

public enum VokiType
{
    General,
    TierList,
    Scoring
}

public static class VokiTypeExtensions
{
    public static TResult Match<TResult>(
        this VokiType type,
        Func<TResult> onGeneral,
        Func<TResult> onTierList,
        Func<TResult> onScoring
    ) => type switch {
        VokiType.General => onGeneral(),
        VokiType.TierList => onTierList(),
        VokiType.Scoring => onScoring(),
        _ => throw new InvalidEnumArgumentException(nameof(type), invalidValue: (int)type, enumClass: typeof(VokiType)),
    };
}