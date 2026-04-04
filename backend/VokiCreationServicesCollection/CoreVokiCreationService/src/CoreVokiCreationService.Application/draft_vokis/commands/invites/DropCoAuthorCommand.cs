using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Application.draft_vokis.commands.invites;

public sealed record DropCoAuthorCommand(
    VokiId VokiId,
    AppUserId CoAuthorId
) :
    ICommand<DraftVoki>,
    IWithAuthCheckStep;

internal sealed class DropCoAuthorCommandHandler : ICommandHandler<DropCoAuthorCommand, DraftVoki>
{
    private readonly IDraftVokiRepository _draftVokiRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public DropCoAuthorCommandHandler(IDraftVokiRepository draftVokiRepository, IUserCtxProvider userCtxProvider) {
        _draftVokiRepository = draftVokiRepository;
        _userCtxProvider = userCtxProvider;
    }


    public async Task<ErrOr<DraftVoki>> Handle(DropCoAuthorCommand command, CancellationToken ct) {
        DraftVoki voki = (await _draftVokiRepository.GetByIdForUpdate(command.VokiId, ct))!;
        ErrOrNothing res = voki.DropCoAuthor(command.UserCtx(_userCtxProvider), command.CoAuthorId);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftVokiRepository.Update(voki, ct);
        return voki;
    }
}