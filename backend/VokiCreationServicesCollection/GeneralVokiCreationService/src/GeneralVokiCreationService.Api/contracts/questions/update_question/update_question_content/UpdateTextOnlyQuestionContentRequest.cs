using GeneralVokiCreationService.Application.draft_vokis.commands.questions.update_question_content;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers;

namespace GeneralVokiCreationService.Api.contracts.questions.update_question.update_question_content;

public class UpdateTextOnlyQuestionContentRequest : IUpdateQuestionContentRequest
{
    public Answer[] Answers { get; init; } = [];

    public sealed record Answer(
        string Text,
        ushort Order,
        string[] RelatedResultIds
    ) : IUpdateQuestionContentRequestAnswer;

    public ErrOrNothing Validate() {
        if (IUpdateQuestionContentRequest.ValidateAnswersCount(Answers.Length).IsErr(out var err)) {
            return err;
        }

        ErrOr<TextOnlyUnsavedQuestionContentDto.Answer[]> answersParseRes = IUpdateQuestionContentRequest
            .ParseAnswers<TextOnlyUnsavedQuestionContentDto.Answer, Answer>(
                Answers,
                createParsed: (answer, order, results) => GeneralVokiAnswerText.Create(answer.Text)
                    .Match<ErrOr<TextOnlyUnsavedQuestionContentDto.Answer>>(
                        successFunc: (text) => new TextOnlyUnsavedQuestionContentDto.Answer(text, order, results),
                        errorFunc: (e) => e.WithMessagePrefix($"Error in the answer with order: {order}. Error:")
                    )
            );
        if (answersParseRes.IsErr(out err)) {
            return err;
        }

        ValidatedContent = new TextOnlyUnsavedQuestionContentDto(answersParseRes.AsSuccess());
        return ErrOrNothing.Nothing;
    }

    public BaseUnsavedQuestionContentDto ValidatedContent { get; private set; }
}