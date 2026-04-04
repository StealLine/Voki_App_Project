using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.questions;

public sealed record UpdateQuestionTextCommand(
    VokiId VokiId,
    GeneralVokiQuestionId QuestionId,
    VokiQuestionText NewQuestionText
) :
    ICommand<VokiQuestionText>,
    IWithAuthCheckStep;

internal sealed class UpdateQuestionTextCommandHandler : ICommandHandler<UpdateQuestionTextCommand, VokiQuestionText>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public UpdateQuestionTextCommandHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        IUserCtxProvider userCtxProvider
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<VokiQuestionText>> Handle(UpdateQuestionTextCommand command, CancellationToken ct) {
        DraftGeneralVoki? voki = await _draftGeneralVokisRepository.GetWithQuestionsForUpdate(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki();
        }

        ErrOr<VokiQuestion> res = voki.UpdateQuestionText(
            command.UserCtx(_userCtxProvider),
            command.QuestionId,
            command.NewQuestionText
        );
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokisRepository.Update(voki, ct);
        return res.AsSuccess().Text;
    }
}