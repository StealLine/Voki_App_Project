using System.Collections.Immutable;
using System.Text.RegularExpressions;
using SharedKernel.errs.utils;

namespace VokimiStorageKeysLib.extension;

public readonly struct AudioFileExtension : IFileExtension
{
    public string Value { get; }

    private AudioFileExtension(string value)
    {
        InvalidConstructorArgumentException.ThrowIfErr(this, Validate(value));
        Value = value;
    }


    public static readonly Regex ExtPattern =
        new("[a-z0-9]{1,10}", RegexOptions.Compiled | RegexOptions.CultureInvariant);
    private static readonly ImmutableHashSet<string> WhiteList = ["mp3", "m4a", "wav", "ogg", "webm"];

    public static ErrOrNothing Validate(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return ErrFactory.IncorrectFormat("Extension is empty");
        }

        var norm = Normalize(input);
        if (!ExtPattern.IsMatch(norm))
        {
            return ErrFactory.IncorrectFormat($"Invalid extension '{input}'. Only letters/digits allowed");
        }

        if (!WhiteList.Contains(norm))
        {
            return ErrFactory.IncorrectFormat(
                $"Extension '.{norm}' is not supported",
                $"Supported: {string.Join(", ", WhiteList.Select(e => "." + e))}"
            );
        }

        return ErrOrNothing.Nothing;
    }

    public static ErrOr<AudioFileExtension> Create(string input) => Validate(input).IsErr(out var err)
        ? err
        : new AudioFileExtension(Normalize(input));

    public static string Normalize(string input) =>
        input.Trim().TrimStart('.').ToLowerInvariant();

    public override string ToString() => Value;
    public static readonly AudioFileExtension Mp3 = new("mp3");


    public bool Equals(IFileExtension? other) =>
        other is not null && string.Equals(Value, other.Value, StringComparison.Ordinal);

    public override int GetHashCode() =>
        StringComparer.Ordinal.GetHashCode(Value);
}