using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;
using VokimiStorageKeysLib.concrete_keys.general_voki;
using VokimiStorageKeysLib.temp_keys;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.results;

public sealed record UpdateVokiResultCommand(
    VokiId VokiId,
    GeneralVokiResultId ResultId,
    VokiResultName NewName,
    VokiResultText NewText,
    string? NewImage
) :
    ICommand<VokiResult>,
    IWithAuthCheckStep;

internal sealed class UpdateResultTextCommandHandler : ICommandHandler<UpdateVokiResultCommand, VokiResult>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly IMainStorageBucket _mainStorageBucket;
    private readonly IUserCtxProvider _userCtxProvider;

    public UpdateResultTextCommandHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        IMainStorageBucket mainStorageBucket,
        IUserCtxProvider userCtxProvider
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _mainStorageBucket = mainStorageBucket;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<VokiResult>> Handle(UpdateVokiResultCommand command, CancellationToken ct) {
        DraftGeneralVoki? voki = await _draftGeneralVokisRepository.GetWithResultsForUpdate(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki();
        }

        var aUserCtx = command.UserCtx(_userCtxProvider);
        if (!voki.HasUserAccess(aUserCtx)) {
            return ErrFactory.NoAccess("You do not have access to this Voki");
        }

        var imageRes = await HandleImage(command.NewImage, command.VokiId, command.ResultId, ct);
        if (imageRes.IsErr(out var err)) {
            return err;
        }

        var imageKey = imageRes.AsSuccess();
        ErrOr<VokiResult> res = voki.UpdateResult(aUserCtx, command.ResultId, command.NewName, command.NewText, imageKey);
        if (res.IsErr(out err)) {
            return err;
        }

        await _draftGeneralVokisRepository.Update(voki, ct);
        return res.AsSuccess();
    }

    private async Task<ErrOr<GeneralVokiResultImageKey?>> HandleImage(
        string? resultImageKey,
        VokiId vokiId,
        GeneralVokiResultId resultId,
        CancellationToken ct
    ) {
        if (resultImageKey is null) {
            return ErrOr<GeneralVokiResultImageKey?>.Success(null);
        }

        if (TempImageKey.IsPossiblySuitable(resultImageKey)) {
            return await HandleTempKey(resultImageKey, vokiId, resultId, ct);
        }

        ErrOr<GeneralVokiResultImageKey> creationRes = GeneralVokiResultImageKey.FromString(resultImageKey);
        if (creationRes.IsErr(out var err)) {
            return err;
        }

        var savedKey = creationRes.AsSuccess();
        if (!savedKey.IsWithIds(vokiId, resultId)) {
            return ErrFactory.Conflict("Specified result image key does not belong to this Voki result");
        }

        return savedKey;
    }

    private async Task<ErrOr<GeneralVokiResultImageKey?>> HandleTempKey(
        string stringTempKey, VokiId vokiId, GeneralVokiResultId resultId, CancellationToken ct
    ) {
        ErrOr<TempImageKey> creationRes = TempImageKey.FromString(stringTempKey);
        if (creationRes.IsErr(out var creationErr)) {
            return creationErr;
        }

        var tempKey = creationRes.AsSuccess();
        var savedImageKey = GeneralVokiResultImageKey.CreateForResult(vokiId, resultId, tempKey.Extension);
        ErrOrNothing copyingRes = await _mainStorageBucket.CopyVokiResultImageFromTempToStandard(
            tempKey, savedImageKey, ct
        );
        if (copyingRes.IsErr(out var err)) {
            return err;
        }

        return savedImageKey;
    }
}