using VokimiStorageKeysLib.base_keys;
using VokimiStorageKeysLib.extension;

namespace VokimiStorageKeysLib.concrete_keys.general_voki;

public class GeneralVokiResultImageKey : BaseStorageImageKey
{
    protected override string Value { get; }
    public VokiId VokiId { get; }
    public GeneralVokiResultId ResultId { get; }
    public override ImageFileExtension ImageExtension { get; }

    public GeneralVokiResultImageKey(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(
            this,
            Scheme.IsKeyValid(value, out var vokiId, out var resultId, out var ext)
        );

        VokiId = vokiId;
        ResultId = resultId;
        ImageExtension = ext;
        Value = value;
    }

    public static GeneralVokiResultImageKey CreateForResult(
        VokiId vokiId, GeneralVokiResultId resultId, ImageFileExtension extension
    ) => new($"{KeyConsts.VokisFolder}/{vokiId}/results/{resultId}.{extension}");
    public static ErrOr<GeneralVokiResultImageKey> FromString(string value) {
        if (Scheme.IsKeyValid(value, out _, out _, out _).IsErr(out var err)) {
            return err;
        }

        return new GeneralVokiResultImageKey(value);
    }
    public bool IsWithIds(VokiId expectedVokiId, GeneralVokiResultId expectedResultId) =>
        VokiId == expectedVokiId && ResultId == expectedResultId;

    private static class Scheme
    {
        private static readonly KeyTemplateParser Parser = new(
            $"{KeyConsts.VokisFolder}/<vokiId:id>/results/<resultId:id>.<ext:imageExt>"
        );

        public static ErrOrNothing IsKeyValid(
            string key,
            out VokiId vokiId,
            out GeneralVokiResultId resultId,
            out ImageFileExtension ext
        ) {
            var parseResult = Parser.TryParse(key);
            if (parseResult.IsErr(out var err)) {
                vokiId = default!;
                resultId = default!;
                ext = default;
                return err;
            }

            Dictionary<string, string> parts = parseResult.AsSuccess();

            vokiId = new VokiId(new Guid(parts["vokiId"]));
            resultId = new GeneralVokiResultId(new Guid(parts["resultId"]));
            ext = ImageFileExtension.Create(parts["ext"]).AsSuccess();

            return ErrOrNothing.Nothing;
        }
    }
}