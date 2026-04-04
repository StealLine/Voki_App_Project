using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InfrastructureShared.EfCore.value_converters;

public class ObjectToJsonConverter<T> : ValueConverter<T, string> where T : class
{
    private static readonly JsonSerializerOptions DefaultOptions = new();
    public ObjectToJsonConverter(JsonSerializerOptions? options = null) : base(
        obj => JsonSerializer.Serialize(obj, options ?? DefaultOptions),
        str => JsonSerializer.Deserialize<T>(str, options ?? DefaultOptions)!
    ) { }
}