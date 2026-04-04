using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate;
using SharedKernel.user_ctx;

namespace CoreVokiCreationService.Application.draft_vokis.commands.invites;

public sealed record class AcceptCoAuthorInviteCommand(
    VokiId VokiId
) : ICommand,
    IWithAuthCheckStep;

internal sealed class AcceptCoAuthorInviteCommandHandler : ICommandHandler<AcceptCoAuthorInviteCommand>
{
    private readonly IDraftVokiRepository _draftVokiRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public AcceptCoAuthorInviteCommandHandler(IDraftVokiRepository draftVokiRepository, IUserCtxProvider userCtxProvider) {
        _draftVokiRepository = draftVokiRepository;
        _userCtxProvider = userCtxProvider;
    }


    public async Task<ErrOrNothing> Handle(AcceptCoAuthorInviteCommand command, CancellationToken ct) {
        DraftVoki voki = (await _draftVokiRepository.GetByIdForUpdate(command.VokiId, ct))!;
        ErrOrNothing result = voki.AcceptInvite(command.UserCtx(_userCtxProvider));
        if (result.IsErr(out var err)) {
            return err;
        }

        await _draftVokiRepository.Update(voki, ct);
        return ErrOrNothing.Nothing;
    }
}