using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;

namespace GeneralVokiCreationService.Api.contracts.questions.update_question;

public class UpdateQuestionTextRequest : IRequestWithValidationNeeded
{
    public string NewText { get; init; }
    public ErrOrNothing Validate() => VokiQuestionText.CheckForErr(NewText);
    public VokiQuestionText ParsedText => VokiQuestionText.Create(NewText).AsSuccess();
}