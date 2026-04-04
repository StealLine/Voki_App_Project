// using GeneralVokiCreationService.Application.common;
// using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers;
// using SharedKernel.common.vokis;
// using SharedKernel.common.vokis.general_vokis;
// using VokimiStorageKeysLib;
// using VokimiStorageKeysLib.concrete_keys.general_voki;
// using VokimiStorageKeysLib.extension;
// using VokimiStorageKeysLib.temp_keys;
//
// namespace GeneralVokiCreationService.Application.draft_vokis.commands.answers.auxiliary;
//
// public class DraftVokiAnswerDataSavingService
// {
//     private readonly IMainStorageBucket _mainStorageBucket;
//
//     public DraftVokiAnswerDataSavingService(IMainStorageBucket mainStorageBucket) {
//         _mainStorageBucket = mainStorageBucket;
//     }
//
//     public Task<ErrOr<BaseVokiAnswerTypeData>> SaveAnswerData(
//         VokiId vokiId,
//         GeneralVokiQuestionId questionId,
//         VokiAnswerTypeDataDto d,
//         CancellationToken ct
//     ) => 
//
//

//
//
//     private async Task<ErrOr<BaseVokiAnswerTypeData>> CreateAudioOnlyAnswerData(
//         VokiId vokiId,
//         GeneralVokiQuestionId questionId,
//         VokiAnswerTypeDataDto answer,
//         CancellationToken ct
//     ) => (
//         await HandleAnswerAudioKey(vokiId, questionId, answer.Audio, ct)
//     ).Bind<BaseVokiAnswerTypeData>(savedKey => new BaseVokiAnswerTypeData.AudioOnly(savedKey));
//
//     private async Task<ErrOr<BaseVokiAnswerTypeData>> CreateAudioAndTextAnswerData(
//         VokiId vokiId,
//         GeneralVokiQuestionId questionId,
//         VokiAnswerTypeDataDto answer,
//         CancellationToken ct
//     ) {
//         ErrOr<GeneralVokiAnswerText> textRes = answer.GetText();
//         if (textRes.IsErr(out var err)) {
//             return err;
//         }
//
//         ErrOr<GeneralVokiAnswerAudioKey> keyRes = await HandleAnswerAudioKey(vokiId, questionId, answer.Audio, ct);
//         if (keyRes.IsErr(out err)) {
//             return err;
//         }
//
//         return new BaseVokiAnswerTypeData.AudioAndText(textRes.AsSuccess(), keyRes.AsSuccess());
//     }
//
//     private async Task<ErrOr<GeneralVokiAnswerAudioKey>> HandleAnswerAudioKey(
//         VokiId vokiId,
//         GeneralVokiQuestionId questionId,
//         string? audio,
//         CancellationToken ct
//     ) {
//         if (audio is null) {
//             return ErrFactory.NoValue.Common("Audio value is not provided");
//         }
//
//
//         if (GeneralVokiAnswerAudioKey.FromString(audio).IsSuccess(out var savedKey)) {
//             return savedKey;
//         }
//
//         ErrOr<TempAudioKey> tempKeyCreationRes = TempAudioKey.FromString(audio);
//         if (tempKeyCreationRes.IsErr(out var err)) {
//             return err;
//         }
//
//         TempAudioKey tempKey = tempKeyCreationRes.AsSuccess();
//         AudioFileExtension ext = tempKey.Extension;
//         GeneralVokiAnswerAudioKey destination = GeneralVokiAnswerAudioKey.CreateForAnswer(vokiId, questionId, ext);
//         ErrOrNothing copyingRes =
//             await _mainStorageBucket.CopyVokiAnswerAudioFromTempToStandard(tempKey, destination, ct);
//         if (copyingRes.IsErr(out err)) {
//             return ErrFactory.Unspecified("Couldn't save answer audio", details: err.Message);
//         }
//
//         return destination;
//     }
// }