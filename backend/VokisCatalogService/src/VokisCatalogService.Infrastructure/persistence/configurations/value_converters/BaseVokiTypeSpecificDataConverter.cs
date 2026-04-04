using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.common.vokis;
using VokisCatalogService.Domain.voki_aggregate;
using VokisCatalogService.Domain.voki_aggregate.type_specific_data;

namespace VokisCatalogService.Infrastructure.persistence.configurations.value_converters;

internal class BaseVokiTypeSpecificDataConverter : ValueConverter<BaseVokiTypeSpecificData, string>
{
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = false
    };

    public BaseVokiTypeSpecificDataConverter()
        : base(
            data => Serialize(data),
            json => Deserialize(json)
        )
    { }

    private static string Serialize(BaseVokiTypeSpecificData data)
    {
        var type = data switch
        {
            GeneralVokiTypeSpecificData => VokiType.General,
            ScoringVokiTypeSpecificData => VokiType.Scoring,
            TierListVokiTypeSpecificData => VokiType.TierList,
            _ => throw new Exception($"Unknown type specific data type: {data.GetType().Name}")
        };

        return $"{type}:{JsonSerializer.Serialize((object)data, JsonOpts)}";
    }

    private static BaseVokiTypeSpecificData Deserialize(string value)
    {
        var parts = value.Split(':', 2);
        if (parts.Length != 2) throw new Exception("Invalid type specific data format");

        if (!Enum.TryParse<VokiType>(parts[0], out var type))
            throw new Exception($"Unknown type specific data type prefix: {parts[0]}");

        var json = parts[1];

        return type switch
        {
            VokiType.General => JsonSerializer.Deserialize<GeneralVokiTypeSpecificData>(json, JsonOpts)!,
            VokiType.Scoring => JsonSerializer.Deserialize<ScoringVokiTypeSpecificData>(json, JsonOpts)!,
            VokiType.TierList => JsonSerializer.Deserialize<TierListVokiTypeSpecificData>(json, JsonOpts)!,
            _ => throw new Exception($"Unhandled voki type for type specific data: {type}")
        };
    }
}
