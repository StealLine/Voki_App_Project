using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiTakingService.Domain.general_voki_aggregate.questions;

public record class VokiQuestionImagesSet(
    ImmutableArray<GeneralVokiQuestionImageKey> Keys,
    double AspectRatio
);