using System.Collections.Immutable;
using SharedKernel.common.rules;
using SharedKernel.exceptions;

namespace VokiCreationServicesLib.Domain.draft_voki_aggregate;

public class VokiTagsSet : ValueObject
{
    public ImmutableHashSet<VokiTagId> Value { get; }

    private VokiTagsSet(ImmutableHashSet<VokiTagId> value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckTagsSetForErr(value));
        Value = value;
    }

    public static ErrOr<VokiTagsSet> Create(ImmutableHashSet<VokiTagId> tags) {
        if (CheckTagsSetForErr(tags).IsErr(out var err)) {
            return err;
        }

        return new VokiTagsSet(tags);
    }

    public static ErrOrNothing CheckTagsSetForErr(ImmutableHashSet<VokiTagId> tags) {
        if (tags is null) {
            return ErrFactory.NoValue.Common("Tags list is null");
        }

        if (tags.Count > VokiRules.MaxTagsForVokiCount) {
            return ErrFactory.LimitExceeded(
                $"Too many tags selected. Voki cannot have more than {VokiRules.MaxTagsForVokiCount} tags",
                $"Tags selected: {tags.Count}"
            );
        }

        return ErrOrNothing.Nothing;
    }

    public override IEnumerable<object> GetEqualityComponents() => Value;
    public static VokiTagsSet Empty => new(ImmutableHashSet<VokiTagId>.Empty);
}