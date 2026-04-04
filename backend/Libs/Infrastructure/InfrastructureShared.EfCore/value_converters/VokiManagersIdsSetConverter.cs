using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.common.vokis;

namespace InfrastructureShared.EfCore.value_converters;

public class VokiManagersIdsSetConverter : ValueConverter<VokiManagersIdsSet, Guid[]>
{
    public VokiManagersIdsSetConverter() : base(
        ids => ids.ToArray().Select(i => i.Value).ToArray(),
        value => VokiManagersIdsSet.Create(
            value.Select(i => new AppUserId(i)).ToImmutableHashSet()
        ).AsSuccess()
    ) { }
}
