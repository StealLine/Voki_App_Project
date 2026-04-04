using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Application.draft_vokis.commands.invites;

public sealed record InviteCoAuthorCommand(
    VokiId VokiId,
    ImmutableHashSet<AppUserId> UserIdsToInvite
) :
    ICommand<DraftVoki>,
    IWithAuthCheckStep;

internal sealed class InviteCoAuthorCommandHandler :
    ICommandHandler<InviteCoAuthorCommand, DraftVoki>
{
    private readonly IDraftVokiRepository _draftVokiRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public InviteCoAuthorCommandHandler(IDraftVokiRepository draftVokiRepository, IUserCtxProvider userCtxProvider) {
        _draftVokiRepository = draftVokiRepository;
        _userCtxProvider = userCtxProvider;
    }


    public async Task<ErrOr<DraftVoki>> Handle(InviteCoAuthorCommand command, CancellationToken ct) {
        DraftVoki voki = (await _draftVokiRepository.GetByIdForUpdate(command.VokiId, ct))!;
        ErrOrNothing result = voki.InviteCoAuthors(command.UserCtx(_userCtxProvider), command.UserIdsToInvite);
        if (result.IsErr(out var err)) {
            return err;
        }

        await _draftVokiRepository.Update(voki, ct);
        return voki;
    }
}