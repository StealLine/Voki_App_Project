using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.exceptions;

namespace InfrastructureShared.EfCore.value_converters.guid_based_ids;

public class GuidBasedIdConverter<TId> : ValueConverter<TId, Guid> where TId : GuidBasedId
{
    public GuidBasedIdConverter() : base(
        id => id.Value,
        value => GuidToId(value)
    ) { }

    private static TId GuidToId(Guid value) {
        TId? id = (TId?)Activator.CreateInstance(typeof(TId), value);
        if (id is null) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.Unspecified(
                $"Could not parse {nameof(TId)} from {value} in the {nameof(Guid)}"
            ));
        }

        return id!;
    }
}