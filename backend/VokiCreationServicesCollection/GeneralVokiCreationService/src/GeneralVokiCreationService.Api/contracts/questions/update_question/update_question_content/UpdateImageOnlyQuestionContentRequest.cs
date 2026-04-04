using GeneralVokiCreationService.Application.draft_vokis.commands.questions.update_question_content;

namespace GeneralVokiCreationService.Api.contracts.questions.update_question.update_question_content;

public class UpdateImageOnlyQuestionContentRequest : IUpdateQuestionContentRequest
{
    public Answer[] Answers { get; init; } = [];

    public sealed record Answer(
        string Image,
        ushort Order,
        string[] RelatedResultIds
    ) : IUpdateQuestionContentRequestAnswer;

    public ErrOrNothing Validate() {
        if (IUpdateQuestionContentRequest.ValidateAnswersCount(Answers.Length).IsErr(out var err)) {
            return err;
        }

        ErrOr<ImageOnlyUnsavedQuestionContentDto.Answer[]> answersParseRes = IUpdateQuestionContentRequest
            .ParseAnswers<ImageOnlyUnsavedQuestionContentDto.Answer, Answer>(
                Answers,
                createParsed: (answer, order, results) => {
                    if (string.IsNullOrWhiteSpace(answer.Image)) {
                        return ErrFactory.NoValue.Common($"Image key is required for answer {order.Value}");
                    }

                    return new ImageOnlyUnsavedQuestionContentDto.Answer(answer.Image, order, results);
                }
            );
        if (answersParseRes.IsErr(out err)) {
            return err;
        }

        ValidatedContent = new ImageOnlyUnsavedQuestionContentDto(answersParseRes.AsSuccess());
        return ErrOrNothing.Nothing;
    }

    public BaseUnsavedQuestionContentDto ValidatedContent { get; private set; }
}