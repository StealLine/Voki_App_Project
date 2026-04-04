using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.domain.ids;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace VokiCreationServicesLib.Infrastructure.persistence.value_converters;

public class VokiCoAuthorIdsSetConverter : ValueConverter<VokiCoAuthorIdsSet, Guid[]>
{
    public VokiCoAuthorIdsSetConverter() : base(
        ids => ids.ToArray().Select(i => i.Value).ToArray(),
        value => VokiCoAuthorIdsSet.Create(
            value.Select(i => new AppUserId(i)).ToImmutableHashSet()
        ).AsSuccess()
    ) { }
}
