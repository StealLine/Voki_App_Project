using System.Text;
using System.Text.RegularExpressions;
using SharedKernel.errs.utils;
using VokimiStorageKeysLib.extension;

namespace VokimiStorageKeysLib;

public sealed class KeyTemplateParser
{
    private readonly Regex _regex;
    private readonly ImmutableDictionary<string, PlaceholderType> _typesByPlaceholder;

    public KeyTemplateParser(string template) {
        Dictionary<string, PlaceholderType> typesByPlaceholderName = new(StringComparer.Ordinal);
        StringBuilder sb = new();
        int pos = 0;

        foreach (Match m in PlaceholderRegex.Matches(template)) {
            if (m.Index > pos) {
                sb.Append(Regex.Escape(template.AsSpan(pos, m.Index - pos).ToString()));
            }

            string placeholderName = m.Groups["name"].Value;
            string placeholderType = m.Groups["type"].Value;

            if (!SupportedPlaceholderTypes.TryGetValue(placeholderType, out var typeDef)) {
                throw new NotSupportedException($"Unsupported placeholder type: {placeholderType}");
            }

            if (!typesByPlaceholderName.TryAdd(placeholderName, typeDef)) {
                throw new NotSupportedException($"Duplicate placeholder '{placeholderName}' is not allowed");
            }

            sb
                .Append("(?<")
                .Append(placeholderName)
                .Append('>')
                .Append(typeDef.BodyRegex.ToString())
                .Append(')');

            pos = m.Index + m.Length;
        }


        if (pos < template.Length) {
            sb.Append(Regex.Escape(template.Substring(pos)));
        }

        string pattern = "^" + sb + "$";
        _regex = new Regex(pattern, DefaultRegexOptions);
        _typesByPlaceholder = typesByPlaceholderName.ToImmutableDictionary(StringComparer.Ordinal);
    }

    private const RegexOptions DefaultRegexOptions = RegexOptions.Compiled | RegexOptions.CultureInvariant;

    private static readonly Regex PlaceholderRegex = new(
        @"<(?<name>[A-Za-z_][A-Za-z0-9_]*)(?::(?<type>\w+))?>",
        DefaultRegexOptions
    );

    private sealed record PlaceholderType(
        string Name,
        Regex BodyRegex,
        Func<string, bool> Validator
    );

    private const int MaxStrLength = 60;

    private static readonly IReadOnlyDictionary<string, PlaceholderType> SupportedPlaceholderTypes =
        new Dictionary<string, PlaceholderType>(StringComparer.OrdinalIgnoreCase) {
            ["id"] = new(Name: "id", new Regex(@"[0-9a-fA-F\-]{36}", DefaultRegexOptions),
                s => Guid.TryParse(s, out _)
            ),
            ["str"] = new(
                Name: "str", new Regex($@"[^/<>]{{1,{MaxStrLength}}}", DefaultRegexOptions),
                s => !string.IsNullOrWhiteSpace(s) && s.Length <= MaxStrLength
            ),
            ["imageExt"] = new(
                Name: "imageExt", ImageFileExtension.ExtPattern,
                s => ImageFileExtension.Validate(s).IsNothing()
            ),
            ["audioExt"] = new(
                Name: "audioExt", AudioFileExtension.ExtPattern,
                s => AudioFileExtension.Validate(s).IsNothing()
            ),
        };


    public ErrOr<Dictionary<string, string>> TryParse(string value) {
        var match = _regex.Match(value);
        if (!match.Success) {
            return ErrFactory.IncorrectFormat("Key does not match expected format", value);
        }

        Dictionary<string, string> result = new(_typesByPlaceholder.Count, StringComparer.Ordinal);

        foreach (var (name, typeDef) in _typesByPlaceholder) {
            var placeholderValue = match.Groups[name].Value;
            if (!typeDef.Validator(placeholderValue)) {
                return ErrFactory.IncorrectFormat(
                    $"Invalid value '{placeholderValue}' for placeholder '{name}' of type '{typeDef.Name}'"
                );
            }

            result[name] = placeholderValue;
        }

        return result;
    }
}