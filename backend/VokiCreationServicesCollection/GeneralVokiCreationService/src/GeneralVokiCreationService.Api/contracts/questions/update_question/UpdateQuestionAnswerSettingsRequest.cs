using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;

namespace GeneralVokiCreationService.Api.contracts.questions.update_question;

public class UpdateQuestionAnswerSettingsRequest : IRequestWithValidationNeeded
{
    public bool ShuffleAnswers { get; init; }
    public bool IsSingleChoice { get; init; }
    public ushort MinAnswersCountLimit { get; init; }
    public ushort MaxAnswersCountLimit { get; init; }

    public ErrOrNothing Validate() {
        if (IsSingleChoice) {
            return ErrOrNothing.Nothing;
        }

        return QuestionAnswersCountLimit.MultipleChoice(MinAnswersCountLimit, MaxAnswersCountLimit).IsErr(out var err)
            ? err
            : ErrOrNothing.Nothing;
    }

    public QuestionAnswersCountLimit ParsedAnswersCountLimit =>
        IsSingleChoice
            ? QuestionAnswersCountLimit.SingleChoice()
            : QuestionAnswersCountLimit.MultipleChoice(MinAnswersCountLimit, MaxAnswersCountLimit).AsSuccess();
}