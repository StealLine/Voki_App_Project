using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using SharedKernel.common.rules;

namespace SharedKernel.common.vokis;

public class VokiManagersIdsSet : ValueObject
{
    private readonly ImmutableHashSet<AppUserId> _ids;

    private VokiManagersIdsSet(ImmutableHashSet<AppUserId> ids) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(ids));
        _ids = ids;
    }

    public override IEnumerable<object> GetEqualityComponents() => _ids;


    public static ErrOr<VokiManagersIdsSet> Create(ImmutableHashSet<AppUserId> ids) {
        if (CheckForErr(ids).IsErr(out var err)) {
            return err;
        }

        return new VokiManagersIdsSet(ids);
    }

    private static ErrOrNothing CheckForErr(ImmutableHashSet<AppUserId> ids) =>
        ids.Count > VokiRules.MaxManagersCount
            ? ErrFactory.LimitExceeded(
                $"Voki cannot have more than {VokiRules.MaxManagersCount} managers",
                $"Provided count: {ids.Count}"
            )
            : ErrOrNothing.Nothing;

    public static VokiManagersIdsSet Empty => new([]);

    [Pure]
    public bool Contains(AppUserId id) => _ids.Contains(id);

    [Pure]
    public bool Any(Func<AppUserId, bool> predicate) => _ids.Any(predicate);

    public AppUserId[] ToArray() => _ids.ToArray();
    public ImmutableHashSet<AppUserId> ToImmutableHashSet() => _ids.ToImmutableHashSet();
    public int Count() => _ids.Count;

    [Pure]
    public VokiManagersIdsSet Remove(AppUserId id) => new(_ids.Remove(id));
}