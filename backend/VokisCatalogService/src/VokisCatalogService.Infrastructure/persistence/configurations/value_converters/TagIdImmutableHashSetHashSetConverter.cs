using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace VokisCatalogService.Infrastructure.persistence.configurations.value_converters;

internal class TagIdImmutableHashSetHashSetConverter : ValueConverter<ImmutableHashSet<VokiTagId>, string[]>
{
    public TagIdImmutableHashSetHashSetConverter()
        : base(
            tags => tags.Select(id => id.ToString()).ToArray(),
            strings => strings.Select(s => new VokiTagId(s)).ToImmutableHashSet()
        ) { }
}

internal class TagIdImmutableHashSetHashSetComparer : ValueComparer<ImmutableHashSet<VokiTagId>>
{
    public TagIdImmutableHashSetHashSetComparer() : base(
        (t1, t2) => t1!.SequenceEqual(t2!),
        t => t.Select(x => x!.GetHashCode()).Aggregate((x, y) => x ^ y),
        t => t.ToImmutableHashSet()
    ) { }
}