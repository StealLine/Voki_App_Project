using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiCreationService.Api.contracts.questions;

public class AddNewQuestionToVokiRequest : IRequestWithValidationNeeded
{
    public  GeneralVokiQuestionContentType QuestionContentType { get; init; }
    public ErrOrNothing Validate() => ErrOrNothing.Nothing;
}