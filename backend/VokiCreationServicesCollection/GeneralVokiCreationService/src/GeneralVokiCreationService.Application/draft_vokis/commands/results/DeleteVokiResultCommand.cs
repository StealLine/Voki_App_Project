namespace GeneralVokiCreationService.Application.draft_vokis.commands.results;

public sealed record DeleteVokiResultCommand(
    VokiId VokiId,
    GeneralVokiResultId ResultId
) :
    ICommand,
    IWithAuthCheckStep;

internal sealed class DeleteVokiResultCommandHandler : ICommandHandler<DeleteVokiResultCommand>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public DeleteVokiResultCommandHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        IUserCtxProvider userCtxProvider
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _userCtxProvider = userCtxProvider;
    }


    public async Task<ErrOrNothing> Handle(
        DeleteVokiResultCommand command, CancellationToken ct
    ) {
        DraftGeneralVoki? voki = await _draftGeneralVokisRepository.GetWithResultsForUpdate(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki();
        }

        var res = voki.DeleteResult(command.UserCtx(_userCtxProvider), command.ResultId);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokisRepository.Update(voki, ct);
        return ErrOrNothing.Nothing;
    }
}