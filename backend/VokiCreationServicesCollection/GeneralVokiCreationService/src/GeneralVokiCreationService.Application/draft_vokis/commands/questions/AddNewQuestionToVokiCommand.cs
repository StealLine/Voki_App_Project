using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.questions;

public sealed record AddNewQuestionToVokiCommand(
    VokiId VokiId,
    GeneralVokiQuestionContentType ContentType
) :
    ICommand<GeneralVokiQuestionId>,
    IWithAuthCheckStep;

internal sealed class AddNewQuestionToVokiCommandHandler :
    ICommandHandler<AddNewQuestionToVokiCommand, GeneralVokiQuestionId>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public AddNewQuestionToVokiCommandHandler(IDraftGeneralVokisRepository draftGeneralVokisRepository,
        IUserCtxProvider userCtxProvider) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _userCtxProvider = userCtxProvider;
    }


    public async Task<ErrOr<GeneralVokiQuestionId>> Handle(AddNewQuestionToVokiCommand command, CancellationToken ct) {
        DraftGeneralVoki? voki = await _draftGeneralVokisRepository.GetWithQuestionsForUpdate(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki();
        }

        var res = voki.AddNewQuestion(command.UserCtx(_userCtxProvider), command.ContentType);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokisRepository.Update(voki, ct);
        return res.AsSuccess();
    }
}