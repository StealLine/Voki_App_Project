using GeneralVokiCreationService.Application.draft_vokis.commands.questions.update_question_content;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers;
using SharedKernel.common;

namespace GeneralVokiCreationService.Api.contracts.questions.update_question.update_question_content;

public class UpdateColorAndTextQuestionContentRequest : IUpdateQuestionContentRequest
{
    public Answer[] Answers { get; init; } = [];

    public sealed record Answer(
        string Text,
        string Color,
        ushort Order,
        string[] RelatedResultIds
    ) : IUpdateQuestionContentRequestAnswer;

    public ErrOrNothing Validate() {
        if (IUpdateQuestionContentRequest.ValidateAnswersCount(Answers.Length).IsErr(out var err)) {
            return err;
        }

        ErrOr<ColorAndTextUnsavedQuestionContentDto.Answer[]> answersParseRes = IUpdateQuestionContentRequest
            .ParseAnswers<ColorAndTextUnsavedQuestionContentDto.Answer, Answer>(
                Answers,
                createParsed: (answer, order, results) => {
                    var textRes = GeneralVokiAnswerText.Create(answer.Text);
                    if (textRes.IsErr(out var e)) {
                        return e.WithMessagePrefix($"Error in answer text (order {order}):");
                    }

                    var colorRes = HexColor.Create(answer.Color);
                    if (colorRes.IsErr(out e)) {
                        return e.WithMessagePrefix($"Error in answer color (order {order}):");
                    }

                    return new ColorAndTextUnsavedQuestionContentDto.Answer(
                        textRes.AsSuccess(), colorRes.AsSuccess(), order, results
                    );
                }
            );
        if (answersParseRes.IsErr(out err)) {
            return err;
        }

        ValidatedContent = new ColorAndTextUnsavedQuestionContentDto(answersParseRes.AsSuccess());
        return ErrOrNothing.Nothing;
    }

    public BaseUnsavedQuestionContentDto ValidatedContent { get; private set; }
}