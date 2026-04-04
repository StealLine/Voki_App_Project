using SharedKernel.errs.utils;
using VokimiStorageKeysLib.extension;

namespace VokimiStorageKeysLib.temp_keys;

public class TempAudioKey : ITempKey
{
    public string Value { get; }
    public AudioFileExtension Extension { get; }
    IFileExtension ITempKey.Extension => Extension;
    private const string TypeSpecificSubFolder = "audio";

    private TempAudioKey(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(
            this, CheckAndExtractExtension(value, out var ext)
        );
        Value = value;
        Extension = ext;
    }

    public static TempAudioKey CreateNew(AudioFileExtension ext) => new(
        $"{ITempKey.TempFolder}/{TypeSpecificSubFolder}/{Guid.NewGuid()}.{ext}"
    );

    public static bool IsPossiblySuitable(string val) =>
        val.Split('/') is [ITempKey.TempFolder, TypeSpecificSubFolder, _];

    public static ErrOr<TempAudioKey> FromString(string value) =>
        CheckAndExtractExtension(value, out _).IsErr(out var err)
            ? err
            : new TempAudioKey(value);

    private static ErrOrNothing CheckAndExtractExtension(string value, out AudioFileExtension ext) {
        ext = default;

        if (string.IsNullOrWhiteSpace(value)) {
            return ErrFactory.IncorrectFormat("Key cannot be null or empty");
        }

        var parts = value.Split('/');
        if (parts.Length != 3) {
            return ErrFactory.IncorrectFormat($"Key must be in format '{{temp_folder}}/{{type_specific}}/filename.ext'");
        }

        if (!string.Equals(parts[0], ITempKey.TempFolder, StringComparison.Ordinal)) {
            return ErrFactory.IncorrectFormat($"Key must start with '{ITempKey.TempFolder}/'");
        }

        if (!string.Equals(parts[1], TypeSpecificSubFolder, StringComparison.Ordinal)) {
            return ErrFactory.IncorrectFormat(
                $"Provided type specific subfolder is incorrect. Correct name is '{TypeSpecificSubFolder}'"
            );
        }

        var fileName = parts[2];
        var dotIndex = fileName.LastIndexOf('.');
        if (dotIndex <= 0 || dotIndex == fileName.Length - 1) {
            return ErrFactory.IncorrectFormat("Key must contain a valid filename and extension");
        }

        var namePart = fileName[..dotIndex];
        var extPart = fileName[(dotIndex + 1)..];

        if (string.IsNullOrWhiteSpace(namePart) || namePart.Length > 150) {
            return ErrFactory.IncorrectFormat("Filename part must not be empty or longer than 150 characters");
        }

        var typedExt = AudioFileExtension.Create(extPart);
        if (typedExt.IsErr(out var err)) {
            return err;
        }

        ext = typedExt.AsSuccess();
        return ErrOrNothing.Nothing;
    }

    public bool Equals(ITempKey? other) =>
        other is not null && string.Equals(Value, other.Value, StringComparison.Ordinal);

    public override string ToString() => Value;
}