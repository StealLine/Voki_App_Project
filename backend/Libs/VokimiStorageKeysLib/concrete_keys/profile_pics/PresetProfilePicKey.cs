using SharedKernel.errs.utils;
using VokimiStorageKeysLib.base_keys;
using VokimiStorageKeysLib.extension;

namespace VokimiStorageKeysLib.concrete_keys.profile_pics;

public class PresetProfilePicKey : BaseStorageImageKey
{
    protected override string Value { get; }
    public override ImageFileExtension ImageExtension => Scheme.AllowedExt;

    private PresetProfilePicKey(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(
            this,
            Scheme.IsKeyValid(value)
        );

        Value = value;
    }

    public static ErrOr<PresetProfilePicKey> CreateFromString(string value) =>
        Scheme.IsKeyValid(value).IsErr(out var err)
            ? err
            : new PresetProfilePicKey(value);

    public static bool IsStringPresetKey(string key) =>
        Scheme.IsKeyValid(key).IsNothing();

    public static  PresetProfilePicKey DefaultProfilePic =>
        new($"{KeyConsts.PresetProfilePicsFolder}/pets-ya-cat.webp");

    private static class Scheme
    {
        private static readonly string Prefix = $"{KeyConsts.PresetProfilePicsFolder}/";
        public static readonly ImageFileExtension AllowedExt = ImageFileExtension.Webp;
        private static readonly string ExtSuffix = $".{AllowedExt}";

        private static readonly HashSet<string> AllowedNames = new(StringComparer.Ordinal) {
            "basic-black",
            //
            "pets-marsita",
            "pets-ya-cat",
            "pets-zara",
            "pets-monika",
            //
            "boykisser-1",
            "boykisser-2",
            "boykisser-3",
            "boykisser-4",
            //
            "dasha-1",
            "dasha-2",
            "dasha-3",
            "dasha-4",
        };


        public static ErrOrNothing IsKeyValid(string key) {
            if (string.IsNullOrWhiteSpace(key)) {
                return ErrFactory.NoValue.Common("Key is empty");
            }

            if (!key.StartsWith(Prefix)) {
                return ErrFactory.IncorrectFormat(
                    "Invalid preset profile pic key format",
                    $"Expected to start with '{Prefix}'. Got: '{key}'"
                );
            }

            if (!key.EndsWith(ExtSuffix)) {
                return ErrFactory.IncorrectFormat(
                    "Invalid preset profile pic key format",
                    $"Expected extension '{ExtSuffix}'. Got: '{key}'"
                );
            }


            if (key.AsSpan(Prefix.Length).Contains('.')) {
                int firstDot = key.IndexOf('.', Prefix.Length);
                if (firstDot < key.Length - ExtSuffix.Length) {
                    return ErrFactory.IncorrectFormat(
                        "Invalid preset profile pic key format",
                        "Key contains extra '.' before extension"
                    );
                }
            }

            int nameLen = key.Length - Prefix.Length - ExtSuffix.Length;
            if (nameLen <= 0) {
                return ErrFactory.IncorrectFormat(
                    "Invalid preset profile pic key format",
                    "Picture name part is empty"
                );
            }

            string extractedName = key.Substring(Prefix.Length, nameLen);

            if (!AllowedNames.Contains(extractedName)) {
                return ErrFactory.ValueOutOfRange(
                    $"Unknown preset name: '{extractedName}'",
                    $"Allowed: {string.Join(", ", AllowedNames)}"
                );
            }

            return ErrOrNothing.Nothing;
        }
    }
}