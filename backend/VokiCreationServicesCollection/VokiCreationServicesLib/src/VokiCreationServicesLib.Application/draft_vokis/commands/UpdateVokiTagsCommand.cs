using VokiCreationServicesLib.Application.common;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace VokiCreationServicesLib.Application.draft_vokis.commands;

public sealed record UpdateVokiTagsCommand(
    VokiId VokiId,
    VokiTagsSet NewTags
) :
    ICommand<VokiTagsSet>,
    IWithAuthCheckStep;

internal sealed class UpdateVokiTagsCommandHandler : ICommandHandler<UpdateVokiTagsCommand, VokiTagsSet>
{
    private readonly IDraftVokiRepository _draftVokiRepository;
    private readonly IUserCtxProvider _userCtxProvider;


    public UpdateVokiTagsCommandHandler(IDraftVokiRepository draftVokiRepository, IUserCtxProvider userCtxProvider) {
        _draftVokiRepository = draftVokiRepository;
        _userCtxProvider = userCtxProvider;
    }


    public async Task<ErrOr<VokiTagsSet>> Handle(UpdateVokiTagsCommand command, CancellationToken ct) {
        BaseDraftVoki? voki = await _draftVokiRepository.GetByIdForUpdate(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki();
        }

        ErrOrNothing res = voki.UpdateTags(command.UserCtx(_userCtxProvider), command.NewTags);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftVokiRepository.Update(voki, ct);
        return voki.Tags;
    }
}