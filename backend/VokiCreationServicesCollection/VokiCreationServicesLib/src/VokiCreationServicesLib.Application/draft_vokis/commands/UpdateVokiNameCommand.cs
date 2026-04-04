using SharedKernel.common.vokis;
using VokiCreationServicesLib.Application.common;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace VokiCreationServicesLib.Application.draft_vokis.commands;

public sealed record UpdateVokiNameCommand(
    VokiId VokiId,
    VokiName NewVokiName
) : ICommand<VokiName>,
    IWithAuthCheckStep;

internal sealed class UpdateVokiNameCommandHandler : ICommandHandler<UpdateVokiNameCommand, VokiName>
{
    private readonly IDraftVokiRepository _draftVokiRepository;
    private readonly IUserCtxProvider _userCtxProvider;


    public UpdateVokiNameCommandHandler(IDraftVokiRepository draftVokiRepository, IUserCtxProvider userCtxProvider) {
        _draftVokiRepository = draftVokiRepository;
        _userCtxProvider = userCtxProvider;
    }


    public async Task<ErrOr<VokiName>> Handle(UpdateVokiNameCommand command, CancellationToken ct) {
        BaseDraftVoki? voki = await _draftVokiRepository.GetByIdForUpdate(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki();
        }

        ErrOrNothing res = voki.UpdateName(command.UserCtx(_userCtxProvider), command.NewVokiName);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftVokiRepository.Update(voki, ct);
        return voki.Name;
    }
}