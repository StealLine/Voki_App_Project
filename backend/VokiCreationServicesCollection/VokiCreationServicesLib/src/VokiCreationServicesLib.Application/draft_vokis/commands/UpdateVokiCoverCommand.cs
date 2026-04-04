using VokiCreationServicesLib.Application.common;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;
using VokimiStorageKeysLib.concrete_keys;
using VokimiStorageKeysLib.temp_keys;

namespace VokiCreationServicesLib.Application.draft_vokis.commands;

public sealed record UpdateVokiCoverCommand(
    VokiId VokiId,
    TempImageKey CoverKey
) : ICommand<VokiCoverKey>,
    IWithAuthCheckStep;

internal sealed class UpdateVokiCoverCommandHandler : ICommandHandler<UpdateVokiCoverCommand, VokiCoverKey>
{
    private readonly IVokiCreationLibMainStorageBucket _vokiCreationLibMainStorageBucket;
    private readonly IDraftVokiRepository _draftVokiRepository;
    private readonly IUserCtxProvider _userCtxProvider;


    public UpdateVokiCoverCommandHandler(
        IVokiCreationLibMainStorageBucket vokiCreationLibMainStorageBucket,
        IDraftVokiRepository draftVokiRepository,
        IUserCtxProvider userCtxProvider
    ) {
        _vokiCreationLibMainStorageBucket = vokiCreationLibMainStorageBucket;
        _draftVokiRepository = draftVokiRepository;
        _userCtxProvider = userCtxProvider;
    }


    public async Task<ErrOr<VokiCoverKey>> Handle(UpdateVokiCoverCommand command, CancellationToken ct) {
        BaseDraftVoki? voki = await _draftVokiRepository.GetByIdForUpdate(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki();
        }

        var aUserCtx = command.UserCtx(_userCtxProvider);
        if (!voki.HasUserAccess(aUserCtx)) {
            return ErrFactory.NoAccess("You do not have access to this Voki");
        }

        VokiCoverKey destination = VokiCoverKey.CreateWithId(command.VokiId, command.CoverKey.Extension);
        ErrOrNothing copyingRes = await _vokiCreationLibMainStorageBucket.CopyVokiCoverFromTempToStandard(
            command.CoverKey, destination, ct
        );
        if (copyingRes.IsErr(out var err)) {
            return err;
        }

        var coverUpdateRes = voki.UpdateCover(aUserCtx, destination);
        if (coverUpdateRes.IsErr(out err)) {
            return err;
        }

        await _draftVokiRepository.Update(voki, ct);
        return voki.Cover;
    }
}