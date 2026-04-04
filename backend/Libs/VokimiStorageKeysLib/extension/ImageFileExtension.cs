using System.Text.RegularExpressions;
using SharedKernel.errs.utils;

namespace VokimiStorageKeysLib.extension;

public readonly struct ImageFileExtension : IFileExtension
{
    public string Value { get; }

    private ImageFileExtension(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, Validate(value));
        Value = value;
    }


    public static readonly Regex ExtPattern =
        new("[a-z0-9]{1,10}", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static readonly ImmutableHashSet<string> WhiteList = ["jpeg", "jpg", "webp", "png"];

    public static ErrOrNothing Validate(string input) {
        if (string.IsNullOrWhiteSpace(input)) {
            return ErrFactory.IncorrectFormat("Extension is empty");
        }

        var norm = Normalize(input);
        if (!ExtPattern.IsMatch(norm)) {
            return ErrFactory.IncorrectFormat($"Invalid extension '{input}'. Only letters/digits allowed");
        }

        if (!WhiteList.Contains(norm)) {
            return ErrFactory.IncorrectFormat(
                $"Extension '.{norm}' is not supported",
                $"Supported: {string.Join(", ", WhiteList.Select(e => "." + e))}"
            );
        }

        return ErrOrNothing.Nothing;
    }

    public static ErrOr<ImageFileExtension> Create(string input) => Validate(input).IsErr(out var err)
        ? err
        : new ImageFileExtension(Normalize(input));

    public static string Normalize(string input) =>
        input.Trim().TrimStart('.').ToLowerInvariant();

    public override string ToString() => Value;
    public static readonly ImageFileExtension Jpg = new("jpg");
    public static readonly ImageFileExtension Jpeg = new("jpeg");
    public static readonly ImageFileExtension Png = new("png");
    public static readonly ImageFileExtension Webp = new("webp");


    public bool Equals(IFileExtension? other) =>
        other is not null && string.Equals(Value, other.Value, StringComparison.Ordinal);

    public override int GetHashCode() =>
        StringComparer.Ordinal.GetHashCode(Value);
}