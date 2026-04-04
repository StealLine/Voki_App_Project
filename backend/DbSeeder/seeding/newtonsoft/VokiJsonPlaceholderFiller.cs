using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DbSeeder.seeding.newtonsoft;

public static class VokiJsonPlaceholderFiller
{
    private static readonly Regex PlaceholderRegex =
        new(@"__([A-Z0-9_]+)__", RegexOptions.Compiled);

    public static string FillPlaceholders(string json) {
        JToken root = JToken.Parse(json);

        Dictionary<string, string> generated = new();

        ReplaceInToken(root, generated);

        return root.ToString(Formatting.Indented);
    }

    private static void ReplaceInToken(JToken token, Dictionary<string, string> generated) {
        List<JValue> stringNodes = new();

        CollectStringNodes(token, stringNodes);

        foreach (var jValue in stringNodes) {
            string str = jValue.Value<string>()!;
            var match = PlaceholderRegex.Match(str);

            if (!match.Success) {
                continue;
            }

            string placeholder = match.Groups[1].Value;

            if (!generated.TryGetValue(placeholder, out string? value)) {
                value = Guid.CreateVersion7(DateTimeOffset.UtcNow).ToString();
                generated[placeholder] = value;
            }

            jValue.Replace(value);
        }
    }

    private static void CollectStringNodes(JToken token, List<JValue> result) {
        if (token.Type == JTokenType.Object) {
            foreach (var prop in token.Children<JProperty>()) {
                CollectStringNodes(prop.Value, result);
            }
        }
        else if (token.Type == JTokenType.Array) {
            foreach (var item in token.Children()) {
                CollectStringNodes(item, result);
            }
        }
        else if (token.Type == JTokenType.String) {
            string value = token.Value<string>()!;

            if (PlaceholderRegex.IsMatch(value)) {
                result.Add((JValue)token);
            }
        }
    }
}