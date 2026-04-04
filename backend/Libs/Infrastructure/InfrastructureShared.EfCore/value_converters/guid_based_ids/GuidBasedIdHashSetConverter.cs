using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InfrastructureShared.EfCore.value_converters.guid_based_ids;

internal class GuidBasedIdHashSetConverter<T> : ValueConverter<HashSet<T>, Guid[]>
    where T : GuidBasedId
{
    public GuidBasedIdHashSetConverter()
        : base(
            ids => ids.Select(id => id.Value).ToArray(),
            guids => guids.Select(guid => (T)Activator.CreateInstance(typeof(T), guid)!)
                .ToHashSet()
        ) { }
}

internal class GuidBasedIdHashSetComparer<T> : ValueComparer<HashSet<T>> where T : GuidBasedId
{
    public GuidBasedIdHashSetComparer() : base(
        (t1, t2) => t1!.SequenceEqual(t2!),
        t => t.Select(x => x!.GetHashCode()).Aggregate((x, y) => x ^ y),
        t => t.ToHashSet()
    ) { }
}