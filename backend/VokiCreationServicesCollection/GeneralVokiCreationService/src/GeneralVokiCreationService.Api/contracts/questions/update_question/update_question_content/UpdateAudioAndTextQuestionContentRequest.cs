using GeneralVokiCreationService.Application.draft_vokis.commands.questions.update_question_content;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers;

namespace GeneralVokiCreationService.Api.contracts.questions.update_question.update_question_content;

public class UpdateAudioAndTextQuestionContentRequest : IUpdateQuestionContentRequest
{
    public Answer[] Answers { get; init; } = [];

    public sealed record Answer(
        string Text,
        string Audio,
        ushort Order,
        string[] RelatedResultIds
    ) : IUpdateQuestionContentRequestAnswer;

    public ErrOrNothing Validate() {
        if (IUpdateQuestionContentRequest.ValidateAnswersCount(Answers.Length).IsErr(out var err)) {
            return err;
        }

        ErrOr<AudioAndTextUnsavedQuestionContentDto.Answer[]> answersParseRes = IUpdateQuestionContentRequest
            .ParseAnswers<AudioAndTextUnsavedQuestionContentDto.Answer, Answer>(
                Answers,
                createParsed: (answer, order, results) => {
                    if (string.IsNullOrWhiteSpace(answer.Audio)) {
                        return ErrFactory.NoValue.Common($"Audio key is required for answer {order.Value}");
                    }

                    return GeneralVokiAnswerText.Create(answer.Text)
                        .Match<ErrOr<AudioAndTextUnsavedQuestionContentDto.Answer>>(
                            successFunc: (text) =>
                                new AudioAndTextUnsavedQuestionContentDto.Answer(text, answer.Audio, order, results),
                            errorFunc: (e) => e.WithMessagePrefix($"Error in the answer with order: {order}. Error:")
                        );
                }
            );
        if (answersParseRes.IsErr(out err)) {
            return err;
        }

        ValidatedContent = new AudioAndTextUnsavedQuestionContentDto(answersParseRes.AsSuccess());
        return ErrOrNothing.Nothing;
    }

    public BaseUnsavedQuestionContentDto ValidatedContent { get; private set; }
}