using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using VokimiStorageKeysLib.concrete_keys.general_voki;
using VokimiStorageKeysLib.temp_keys;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.questions;

public sealed record UpdateQuestionImageSetCommand(
    VokiId VokiId,
    GeneralVokiQuestionId QuestionId,
    HashSet<TempImageKey> TempKeys,
    HashSet<GeneralVokiQuestionImageKey> SavedKeys,
    VokiQuestionImagesAspectRatio ImagesAspectRatio
) :
    ICommand<VokiQuestionImagesSet>,
    IWithAuthCheckStep,
    IWithBasicValidationStep
{
    public ErrOrNothing Validate() {
        int totalCount = TempKeys.Count + SavedKeys.Count;
        if (VokiQuestionImagesSet.CheckForErr(totalCount).IsErr(out var err)) {
            return err;
        }

        if (SavedKeys.Any(k => k.QuestionId != QuestionId)) {
            return ErrFactory.Conflict("Not all saved images belong to this question");
        }

        return ErrOrNothing.Nothing;
    }
}

internal sealed class UpdateQuestionImageSetCommandHandler :
    ICommandHandler<UpdateQuestionImageSetCommand, VokiQuestionImagesSet>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly IMainStorageBucket _mainStorageBucket;
    private readonly IUserCtxProvider _userCtxProvider;


    public UpdateQuestionImageSetCommandHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        IMainStorageBucket mainStorageBucket,
        IUserCtxProvider userCtxProvider
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _mainStorageBucket = mainStorageBucket;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<VokiQuestionImagesSet>> Handle(UpdateQuestionImageSetCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokisRepository.GetWithQuestionsForUpdate(command.VokiId, ct))!;

        List<GeneralVokiQuestionImageKey> resultKeys = [..command.SavedKeys];

        if (command.TempKeys.Count > 0) {
            Dictionary<TempImageKey, GeneralVokiQuestionImageKey> tempToDest = command.TempKeys.ToDictionary(
                k => k,
                k => GeneralVokiQuestionImageKey.CreateForQuestion(command.VokiId, command.QuestionId, k.Extension)
            );


            ErrOrNothing copyingRes = await _mainStorageBucket.CopyVokiQuestionImagesFromTempToStandard(tempToDest, ct);
            if (copyingRes.IsErr()) {
                return ErrFactory.Unspecified("Couldn't update question images from temp to standard");
            }

            resultKeys.AddRange(tempToDest.Values);
        }

        ErrOr<VokiQuestionImagesSet> imagesSetRes = VokiQuestionImagesSet.Create([.. resultKeys], command.ImagesAspectRatio);
        if (imagesSetRes.IsErr(out var err)) {
            return err;
        }

        ErrOr<VokiQuestionImagesSet> updateRes = voki.UpdateQuestionImages(
            command.UserCtx(_userCtxProvider),
            command.QuestionId,
            imagesSetRes.AsSuccess()
        );
        if (updateRes.IsErr(out err)) {
            return err;
        }

        await _draftGeneralVokisRepository.Update(voki, ct);
        return updateRes.AsSuccess();
    }
}