using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Application.draft_vokis.commands.invites;

public sealed record CancelCoAuthorInviteCommand(VokiId VokiId, AppUserId NewCoAuthorId) :
    ICommand<DraftVoki>,
    IWithAuthCheckStep;

internal sealed class CancelCoAuthorInviteCommandHandler : ICommandHandler<CancelCoAuthorInviteCommand, DraftVoki>
{
    private readonly IDraftVokiRepository _draftVokiRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public CancelCoAuthorInviteCommandHandler(
        IDraftVokiRepository draftVokiRepository,
        IUserCtxProvider userCtxProvider
    ) {
        _draftVokiRepository = draftVokiRepository;
        _userCtxProvider = userCtxProvider;
    }


    public async Task<ErrOr<DraftVoki>> Handle(CancelCoAuthorInviteCommand command, CancellationToken ct) {
        DraftVoki voki = (await _draftVokiRepository.GetByIdForUpdate(command.VokiId, ct))!;
        var result = voki.CancelCoAuthorInvite(command.UserCtx(_userCtxProvider), command.NewCoAuthorId);
        if (result.IsErr(out var err)) {
            return err;
        }

        await _draftVokiRepository.Update(voki, ct);
        return voki;
    }
}