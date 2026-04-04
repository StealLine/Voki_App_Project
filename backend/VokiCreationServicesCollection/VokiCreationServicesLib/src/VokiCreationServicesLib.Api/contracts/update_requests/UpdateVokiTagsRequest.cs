using SharedKernel.common.rules;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace VokiCreationServicesLib.Api.contracts.update_requests;

public class UpdateVokiTagsRequest : IRequestWithValidationNeeded
{
    public string[] NewTags { get; init; }

    public ErrOrNothing Validate() {
        if (NewTags is null) {
            return ErrFactory.NoValue.Common("Tags list is null");
        }

        if (NewTags.Length > VokiRules.MaxTagsForVokiCount) {
            return ErrFactory.LimitExceeded(
                $"Too many tags selected. Voki cannot have more than {VokiRules.MaxTagsForVokiCount} tags",
                $"Tags selected: {NewTags.Length}"
            );
        }

        var incorrectTags = NewTags.Where((t) => !VokiTagId.IsStringValidTag(t)).ToArray();
        if (incorrectTags.Length > 0) {
            return ErrFactory.IncorrectFormat(
                "Some of the selected tags are invalid",
                $"Incorrect tags: {string.Join(", ", incorrectTags)}"
            );
        }

        var tags = VokiTagsSet.Create(NewTags.Select(t => new VokiTagId(t)).ToImmutableHashSet());
        if (tags.IsErr(out var err)) {
            return err;
        }

        ParsedTags = tags.AsSuccess();
        return ErrOrNothing.Nothing;
    }

    public VokiTagsSet ParsedTags { get; private set; }
}