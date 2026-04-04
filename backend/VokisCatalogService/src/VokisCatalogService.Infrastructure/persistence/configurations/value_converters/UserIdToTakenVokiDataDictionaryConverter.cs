using System.Collections.Immutable;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VokisCatalogService.Domain.app_user_aggregate;

namespace VokisCatalogService.Infrastructure.persistence.configurations.value_converters;

internal class UserIdToTakenVokiDataDictionaryConverter :
    ValueConverter<ImmutableDictionary<VokiId, UserTakenVokiData>, string>
{
    private static readonly JsonSerializerOptions JsonOpts = new() {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = false
    };

    public UserIdToTakenVokiDataDictionaryConverter()
        : base(
            dic => JsonSerializer.Serialize(
                dic.ToDictionary(kvp => kvp.Key.ToString(), kvp => kvp.Value), JsonOpts
            ),
            json => JsonSerializer
                .Deserialize<Dictionary<string, UserTakenVokiData>>(json, JsonOpts)!
                .ToImmutableDictionary(
                    kvp => new VokiId(new(kvp.Key)),
                    kvp => kvp.Value
                )
        ) { }
}

internal sealed class UserIdToTakenVokiDataDictionaryComparer
    : ValueComparer<ImmutableDictionary<VokiId, UserTakenVokiData>>
{
    public UserIdToTakenVokiDataDictionaryComparer()
        : base(
            (a, b) => AreEqual(a, b),
            dict => CalculateHashCode(dict),
            dict => dict.ToImmutableDictionary()
        ) { }

    private static bool AreEqual(
        ImmutableDictionary<VokiId, UserTakenVokiData>? a,
        ImmutableDictionary<VokiId, UserTakenVokiData>? b
    ) {
        if (ReferenceEquals(a, b)) {
            return true;
        }

        if (a is null || b is null || a.Count != b.Count) {
            return false;
        }

        foreach (var (key, valueA) in a) {
            if (!b.TryGetValue(key, out var valueB)) {
                return false;
            }

            if (!EqualityComparer<UserTakenVokiData>.Default.Equals(valueA, valueB)) {
                return false;
            }
        }

        return true;
    }

    private static int CalculateHashCode(ImmutableDictionary<VokiId, UserTakenVokiData> dict) {
        HashCode hc = new();
        foreach (var kvp in dict.OrderBy(k => k.Key)) {
            hc.Add(kvp.Key);
            hc.Add(kvp.Value);
        }

        return hc.ToHashCode();
    }
}