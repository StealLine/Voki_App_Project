namespace GeneralVokiCreationService.Application.draft_vokis.commands.questions;

public sealed record MoveQuestionDownInOrderCommand(
    VokiId VokiId,
    GeneralVokiQuestionId QuestionId
) :
    ICommand<ImmutableArray<VokiQuestion>>,   
    IWithAuthCheckStep;

internal sealed class MoveQuestionDownInOrderCommandHandler :
    ICommandHandler<MoveQuestionDownInOrderCommand, ImmutableArray<VokiQuestion>>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public MoveQuestionDownInOrderCommandHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        IUserCtxProvider userCtxProvider
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<ImmutableArray<VokiQuestion>>> Handle(MoveQuestionDownInOrderCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokisRepository.GetWithQuestionsForUpdate(command.VokiId, ct))!;
        
        var aUserCtx = command.UserCtx(_userCtxProvider);
        var res = voki.MoveQuestionDownInOrder(aUserCtx, command.QuestionId);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokisRepository.Update(voki, ct);
        return res.AsSuccess();
    }
}