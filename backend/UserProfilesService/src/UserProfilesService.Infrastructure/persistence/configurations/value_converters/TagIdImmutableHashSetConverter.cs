using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace UserProfilesService.Infrastructure.persistence.configurations.value_converters;

internal class TagIdImmutableHashSetConverter : ValueConverter<ImmutableHashSet<VokiTagId>, string[]>
{
    public TagIdImmutableHashSetConverter()
        : base(
            tags => tags.Select(id => id.ToString()).ToArray(),
            strings => strings.Select(s => new VokiTagId(s)).ToImmutableHashSet()
        ) { }
}

internal class TagIdImmutableHashSetComparer : ValueComparer<ImmutableHashSet<VokiTagId>>
{
    public TagIdImmutableHashSetComparer() : base(
        (t1, t2) => t1!.SequenceEqual(t2!),
        t => t.Select(x => x!.GetHashCode()).Aggregate((x, y) => x ^ y),
        t => t.ToImmutableHashSet()
    ) { }
}