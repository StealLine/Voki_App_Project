using SharedKernel.exceptions;
using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;

public class VokiQuestionImagesSet : ValueObject

{
    public const int MaxImagesForQuestionCount = 5;
    public ImmutableArray<GeneralVokiQuestionImageKey> Keys { get; }
    public VokiQuestionImagesAspectRatio AspectRatio { get; }

    private VokiQuestionImagesSet(
        ImmutableArray<GeneralVokiQuestionImageKey> keys,
        VokiQuestionImagesAspectRatio aspectRatio
    ) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(keys.Length));
        this.Keys = keys;
        AspectRatio = aspectRatio;
    }

    public static VokiQuestionImagesSet Default => new(
        ImmutableArray<GeneralVokiQuestionImageKey>.Empty,
        VokiQuestionImagesAspectRatio.Default
    );

    public override IEnumerable<object> GetEqualityComponents() => [..Keys, AspectRatio];

    public static ErrOr<VokiQuestionImagesSet> Create(
        ImmutableArray<GeneralVokiQuestionImageKey> keys,
        VokiQuestionImagesAspectRatio aspectRatio
    ) => CheckForErr(keys.Length).IsErr(out var err)
        ? err
        : new VokiQuestionImagesSet(keys, aspectRatio);

    public static ErrOrNothing CheckForErr(int count) =>
        count > MaxImagesForQuestionCount
            ? ErrFactory.LimitExceeded(
                $"A question can have at most {MaxImagesForQuestionCount} images",
                $"Current count is {count}")
            : ErrOrNothing.Nothing;
}