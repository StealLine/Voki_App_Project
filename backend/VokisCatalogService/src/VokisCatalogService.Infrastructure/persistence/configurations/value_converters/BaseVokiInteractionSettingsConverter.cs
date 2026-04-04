using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.common.vokis;
using SharedKernel.common.vokis.general_vokis;
using SharedKernel.common.vokis.scoring_voki;
using SharedKernel.common.vokis.tier_list_voki;

namespace VokisCatalogService.Infrastructure.persistence.configurations.value_converters;

internal class BaseVokiInteractionSettingsConverter : ValueConverter<BaseVokiInteractionSettings, string>
{
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = false
    };

    public BaseVokiInteractionSettingsConverter()
        : base(
            settings => Serialize(settings),
            json => Deserialize(json)
        )
    { }

    private static string Serialize(BaseVokiInteractionSettings settings)
    {
        var type = settings switch
        {
            GeneralVokiInteractionSettings => VokiType.General,
            ScoringVokiInteractionSettings => VokiType.Scoring,
            TierListVokiInteractionSettings => VokiType.TierList,
            _ => throw new Exception($"Unknown interaction settings type: {settings.GetType().Name}")
        };

        return $"{type}:{JsonSerializer.Serialize((object)settings, JsonOpts)}";
    }

    private static BaseVokiInteractionSettings Deserialize(string value)
    {
        var parts = value.Split(':', 2);
        if (parts.Length != 2) throw new Exception("Invalid interaction settings format");

        if (!Enum.TryParse<VokiType>(parts[0], out var type))
            throw new Exception($"Unknown interaction settings type prefix: {parts[0]}");

        var json = parts[1];

        return type switch
        {
            VokiType.General => JsonSerializer.Deserialize<GeneralVokiInteractionSettings>(json, JsonOpts)!,
            VokiType.Scoring => JsonSerializer.Deserialize<ScoringVokiInteractionSettings>(json, JsonOpts)!,
            VokiType.TierList => JsonSerializer.Deserialize<TierListVokiInteractionSettings>(json, JsonOpts)!,
            _ => throw new Exception($"Unhandled voki type for interaction settings: {type}")
        };
    }
}
