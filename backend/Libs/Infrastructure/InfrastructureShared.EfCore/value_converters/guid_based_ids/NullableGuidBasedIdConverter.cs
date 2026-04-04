using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.exceptions;

namespace InfrastructureShared.EfCore.value_converters.guid_based_ids;

public class NullableGuidBasedIdConverter<TId> : ValueConverter<TId?, Guid?> where TId : GuidBasedId
{
    public NullableGuidBasedIdConverter() : base(
        id => NullableEntityIdToGuid(id),
        value => value.HasValue ? GuidToId(value.Value) : null
    ) { }

    private static Guid? NullableEntityIdToGuid(GuidBasedId? id) => id?.Value;

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