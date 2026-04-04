using VokiCreationServicesLib.Application.common;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace VokiCreationServicesLib.Application.draft_vokis.commands;

public sealed record UpdateVokiDetailsCommand(
    VokiId VokiId,
    VokiDetails NewDetails
) :
    ICommand<VokiDetails>,
    IWithAuthCheckStep;

internal sealed class UpdateVokiDetailsCommandHandler : ICommandHandler<UpdateVokiDetailsCommand, VokiDetails>
{
    private readonly IDraftVokiRepository _draftVokiRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public UpdateVokiDetailsCommandHandler(
        IDraftVokiRepository draftVokiRepository,
        IUserCtxProvider userCtxProvider
    ) {
        _draftVokiRepository = draftVokiRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<VokiDetails>> Handle(UpdateVokiDetailsCommand command, CancellationToken ct) {
        BaseDraftVoki? voki = await _draftVokiRepository.GetByIdForUpdate(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki();
        }

        var res = voki.UpdateDetails(command.UserCtx(_userCtxProvider), command.NewDetails);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftVokiRepository.Update(voki, ct);
        return voki.Details;
    }
}