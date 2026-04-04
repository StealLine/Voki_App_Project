namespace GeneralVokiCreationService.Application.draft_vokis.commands;

public sealed record UpdateVokiTakingProcessSettingsCommand(
    VokiId VokiId,
    VokiTakingProcessSettings NewSettings
) :
    ICommand<VokiTakingProcessSettings>,
    IWithAuthCheckStep;

internal sealed class UpdateVokiTakingProcessSettingsCommandHandler :
    ICommandHandler<UpdateVokiTakingProcessSettingsCommand, VokiTakingProcessSettings>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly IUserCtxProvider _userCtxProvider;


    public UpdateVokiTakingProcessSettingsCommandHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        IUserCtxProvider userCtxProvider
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<VokiTakingProcessSettings>> Handle(
        UpdateVokiTakingProcessSettingsCommand command, CancellationToken ct
    ) {
        DraftGeneralVoki? voki = await _draftGeneralVokisRepository.GetByIdForUpdate(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki();
        }

        ErrOrNothing res = voki.UpdateTakingProcessSettings(command.UserCtx(_userCtxProvider), command.NewSettings);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokisRepository.Update(voki, ct);
        return voki.TakingProcessSettings;
    }
}