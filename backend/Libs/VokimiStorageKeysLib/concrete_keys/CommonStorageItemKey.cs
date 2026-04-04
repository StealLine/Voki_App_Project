using VokimiStorageKeysLib.base_keys;
using VokimiStorageKeysLib.extension;

namespace VokimiStorageKeysLib.concrete_keys;

public class CommonStorageItemKey : BaseStorageImageKey
{
    protected override string Value { get; }
    public string Name { get; }
    public override ImageFileExtension ImageExtension { get; }

    private CommonStorageItemKey(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(
            this,
            Scheme.IsKeyValid(value, out var name, out var ext)
        );

        Name = name;
        ImageExtension = ext;
        Value = value;
    }

    public static readonly CommonStorageItemKey DefaultVokiCover = new(KeyConsts.DefaultVokiCover);

    private static class Scheme
    {
        private static readonly KeyTemplateParser Parser = new(
            $"{KeyConsts.CommonFolder}/<name:str>.<ext:imageExt>"
        );

        public static ErrOrNothing IsKeyValid(string key, out string name, out ImageFileExtension ext) {
            var parseResult = Parser.TryParse(key);
            if (parseResult.IsErr(out var err)) {
                name = default!;
                ext = default;
                return err;
            }

            var parts = parseResult.AsSuccess();

            name = parts["name"];
            ext = ImageFileExtension.Create(parts["ext"]).AsSuccess();

            return ErrOrNothing.Nothing;
        }
    }
}