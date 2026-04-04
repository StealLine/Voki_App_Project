using VokimiStorageKeysLib.base_keys;
using VokimiStorageKeysLib.extension;
using VokimiStorageKeysLib.temp_keys;

namespace VokimiStorageKeysLib.concrete_keys.general_voki;

public class GeneralVokiAnswerAudioKey : BaseStorageAudioKey
{
    protected override string Value { get; }
    public VokiId VokiId { get; }
    public GeneralVokiQuestionId QuestionId { get; }
    public override AudioFileExtension AudioExtension { get; }

    public GeneralVokiAnswerAudioKey(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(
            this,
            Scheme.IsKeyValid(value, out var vokiId, out var questionId, out var ext)
        );

        VokiId = vokiId;
        QuestionId = questionId;
        AudioExtension = ext;
        Value = value;
    }

    public static GeneralVokiAnswerAudioKey CreateForAnswerFromTemp(
        VokiId vokiId, GeneralVokiQuestionId questionId, TempAudioKey tempKey
    ) => new($"{KeyConsts.VokisFolder}/{vokiId}/questions/{questionId}/answer_audios/{Guid.NewGuid()}.{tempKey.Extension.Value}");

    public static ErrOr<GeneralVokiAnswerAudioKey> FromString(string value) {
        if (Scheme.IsKeyValid(value, out _, out _, out _).IsErr(out var err)) {
            return err;
        }

        return new GeneralVokiAnswerAudioKey(value);
    }

    public bool IsWithIds(VokiId expectedVokiId, GeneralVokiQuestionId expectedQuestionId) =>
        VokiId == expectedVokiId && QuestionId == expectedQuestionId;

    private static class Scheme
    {
        private static readonly KeyTemplateParser Parser = new(
            $"{KeyConsts.VokisFolder}/<vokiId:id>/questions/<questionId:id>/answer_audios/<version:id>.<ext:audioExt>"
        );

        public static ErrOrNothing IsKeyValid(
            string key,
            out VokiId vokiId,
            out GeneralVokiQuestionId questionId,
            out AudioFileExtension ext
        ) {
            var parseResult = Parser.TryParse(key);
            if (parseResult.IsErr(out var err)) {
                vokiId = default!;
                questionId = default!;
                ext = default;
                return err;
            }

            var parts = parseResult.AsSuccess();

            vokiId = new VokiId(new Guid(parts["vokiId"]));
            questionId = new GeneralVokiQuestionId(new Guid(parts["questionId"]));
            ext = AudioFileExtension.Create(parts["ext"]).AsSuccess();

            return ErrOrNothing.Nothing;
        }
    }
}