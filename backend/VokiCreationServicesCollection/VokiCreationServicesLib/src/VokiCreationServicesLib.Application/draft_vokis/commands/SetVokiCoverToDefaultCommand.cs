using VokiCreationServicesLib.Application.common;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;
using VokimiStorageKeysLib.concrete_keys;
using VokimiStorageKeysLib.extension;

namespace VokiCreationServicesLib.Application.draft_vokis.commands;

public sealed record SetVokiCoverToDefaultCommand(
    VokiId VokiId
) :
    ICommand<VokiCoverKey>,
    IWithAuthCheckStep;

internal sealed class SetVokiCoverToDefaultCommandHandler : ICommandHandler<SetVokiCoverToDefaultCommand, VokiCoverKey>
{
    private readonly IDraftVokiRepository _draftVokiRepository;
    private readonly IVokiCreationLibMainStorageBucket _libMainStorageBucket;
    private readonly IUserCtxProvider _userCtxProvider;


    public SetVokiCoverToDefaultCommandHandler(
        IDraftVokiRepository draftVokiRepository,
        IVokiCreationLibMainStorageBucket libMainStorageBucket,
        IUserCtxProvider userCtxProvider
    ) {
        _draftVokiRepository = draftVokiRepository;
        _libMainStorageBucket = libMainStorageBucket;
        _userCtxProvider = userCtxProvider;
    }


    public async Task<ErrOr<VokiCoverKey>> Handle(SetVokiCoverToDefaultCommand command, CancellationToken ct) {
        BaseDraftVoki? voki = await _draftVokiRepository.GetByIdForUpdate(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki();
        }

        ImageFileExtension ext = CommonStorageItemKey.DefaultVokiCover.ImageExtension;
        VokiCoverKey defaultVokiCover = VokiCoverKey.CreateWithId(command.VokiId, ext);
        ErrOrNothing copyRes = await _libMainStorageBucket.CopyDefaultVokiCoverForVoki(defaultVokiCover, ct);
        if (copyRes.IsErr(out var err)) {
            return err;
        }

        var res = voki.UpdateCover(command.UserCtx(_userCtxProvider), defaultVokiCover);
        if (res.IsErr(out err)) {
            return err;
        }

        await _draftVokiRepository.Update(voki, ct);
        return voki.Cover;
    }
}