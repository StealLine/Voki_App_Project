using VokimiStorageKeysLib.base_keys;
using VokimiStorageKeysLib.extension;

namespace VokimiStorageKeysLib.concrete_keys.profile_pics;

public class UserProfilePicKey : BaseStorageImageKey
{
    protected override string Value { get; }
    public AppUserId UserId { get; }
    public override ImageFileExtension ImageExtension { get; }

    public UserProfilePicKey(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(
            this,
            Scheme.IsKeyValid(value, out var userId, out var ext)
        );

        UserId = userId;
        ImageExtension = ext;
        Value = value;
    }

    public static ErrOr<UserProfilePicKey> CreateNewForUser(AppUserId userId, ImageFileExtension extension) {
        var key = $"{KeyConsts.UserProfilePicsFolder}/{userId}.{extension}";

        var validate = Scheme.IsKeyValid(key, out _, out _);
        if (validate.IsErr(out var err)) {
            return err;
        }

        return new UserProfilePicKey(key);
    }

    public static ErrOr<UserProfilePicKey> CreateFromString(string stringKey) =>
        Scheme.IsKeyValid(stringKey, out _, out _).IsErr(out var err)
            ? err
            : new UserProfilePicKey(stringKey);

    public static bool IsStringWithPicsPrefix(string? value) =>
        !string.IsNullOrWhiteSpace(value)
        && value.StartsWith(KeyConsts.UserProfilePicsFolder);

    public bool IsForUser(AppUserId userId) => UserId == userId;

    private static class Scheme
    {
        private static readonly KeyTemplateParser Parser = new(
            $"{KeyConsts.UserProfilePicsFolder}/<userId:id>.<ext:imageExt>"
        );

        public static ErrOrNothing IsKeyValid(string key, out AppUserId userId, out ImageFileExtension ext) {
            var parseResult = Parser.TryParse(key);
            if (parseResult.IsErr(out var err)) {
                userId = default!;
                ext = default;
                return err;
            }

            var parts = parseResult.AsSuccess();

            userId = new AppUserId(new Guid(parts["userId"]));
            ext = ImageFileExtension.Create(parts["ext"]).AsSuccess();

            return ErrOrNothing.Nothing;
        }
    }
}