using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.questions;

public sealed record UpdateQuestionAnswerSettingsCommand(
    VokiId VokiId,
    GeneralVokiQuestionId QuestionId,
    QuestionAnswersCountLimit NewCountLimit,
    bool ShuffleAnswers
) :
    ICommand<VokiQuestion>,
    IWithAuthCheckStep;

internal sealed class UpdateQuestionAnswerSettingsCommandHandler :
    ICommandHandler<UpdateQuestionAnswerSettingsCommand, VokiQuestion>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly IUserCtxProvider _userCtxProvider;


    public UpdateQuestionAnswerSettingsCommandHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        IUserCtxProvider userCtxProvider
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<VokiQuestion>> Handle(UpdateQuestionAnswerSettingsCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokisRepository.GetWithQuestionsForUpdate(command.VokiId, ct))!;
        var res = voki.UpdateQuestionAnswerSettings(
            command.UserCtx(_userCtxProvider),
            command.QuestionId, command.NewCountLimit, command.ShuffleAnswers
        );
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokisRepository.Update(voki, ct);
        return res.AsSuccess();
    }
}