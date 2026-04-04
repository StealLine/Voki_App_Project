using GeneralVokiTakingService.Domain.general_voki_aggregate.questions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.value_converters.vokis;

public class VokiQuestionImagesSetConverter : ValueConverter<VokiQuestionImagesSet, string>
{
    public VokiQuestionImagesSetConverter() : base(
        set => ToString(set),
        str => FromString(str)
    ) { }

    private static string ToString(VokiQuestionImagesSet set) {
        var keysArrayString = string.Join(',', set.Keys.Select(k => k.ToString()));
        return $"{set.AspectRatio}|{keysArrayString}";
    }

    private static VokiQuestionImagesSet FromString(string str) {
        var parts = str.Split('|', 2);
        if (parts.Length != 2) {
            throw new FormatException("Invalid serialized VokiQuestionImagesSet format");
        }

        double aspectRatio = double.Parse(parts[0]);
        ImmutableArray<GeneralVokiQuestionImageKey> keys = [];

        if (!string.IsNullOrWhiteSpace(parts[1])) {
            keys = parts[1]
                .Split(',')
                .Select(k => new GeneralVokiQuestionImageKey(k))
                .ToImmutableArray();
        }


        return new VokiQuestionImagesSet(keys, aspectRatio);
    }
}