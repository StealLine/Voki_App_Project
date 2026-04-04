using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiCreationService.Application.draft_vokis.commands;

public sealed record UpdateVokiInteractionSettingsCommand(
    VokiId VokiId,
    GeneralVokiInteractionSettings NewSettings
) :
    ICommand<GeneralVokiInteractionSettings>,
    IWithAuthCheckStep;

internal sealed class UpdateVokiInteractionSettingsCommandHandler :
    ICommandHandler<UpdateVokiInteractionSettingsCommand, GeneralVokiInteractionSettings>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly IUserCtxProvider _userCtxProvider;


    public UpdateVokiInteractionSettingsCommandHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        IUserCtxProvider userCtxProvider
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<GeneralVokiInteractionSettings>> Handle(
        UpdateVokiInteractionSettingsCommand command, CancellationToken ct
    ) {
        DraftGeneralVoki? voki = await _draftGeneralVokisRepository.GetByIdForUpdate(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki();
        }

        ErrOrNothing res = voki.UpdateInteractionSettings(command.UserCtx(_userCtxProvider), command.NewSettings);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokisRepository.Update(voki, ct);
        return voki.InteractionSettings;
    }
}