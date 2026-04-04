using System.Collections.Immutable;
using System.Text.Json;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiCreationService.Infrastructure.persistence.configurations.value_converters.questions;

public class VokiQuestionImagesSetConverter : ValueConverter<VokiQuestionImagesSet, string>
{
    public VokiQuestionImagesSetConverter() : base(
        set => ToString(set),
        str => FromString(str)
    ) { }

    private static string ToString(VokiQuestionImagesSet set)
    {
        var ratio = $"{set.AspectRatio.Width}:{set.AspectRatio.Height}";
        var keysArray = set.Keys.Select(k => k.ToString()).ToArray();
        var keysJson = JsonSerializer.Serialize(keysArray);
        return $"{ratio}|{keysJson}";
    }

    private static VokiQuestionImagesSet FromString(string str)
    {
        var parts = str.Split('|', 2);
        if (parts.Length != 2)
            throw new FormatException("Invalid serialized VokiQuestionImagesSet format");

        var ratioParts = parts[0].Split(':');
        if (ratioParts.Length != 2)
            throw new FormatException("Invalid aspect ratio format");

        var width = double.Parse(ratioParts[0]);
        var height = double.Parse(ratioParts[1]);

        var keysArray = JsonSerializer.Deserialize<string[]>(parts[1]) ?? [];
        var keyObjs = keysArray.Select(k => new GeneralVokiQuestionImageKey(k)).ToImmutableArray();

        var aspectRatio = VokiQuestionImagesAspectRatio.Create(width, height).AsSuccess();
        return VokiQuestionImagesSet.Create(keyObjs, aspectRatio).AsSuccess();
    }
}
