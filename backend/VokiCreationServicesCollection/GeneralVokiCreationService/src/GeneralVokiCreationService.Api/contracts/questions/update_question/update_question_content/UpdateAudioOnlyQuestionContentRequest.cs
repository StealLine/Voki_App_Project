using GeneralVokiCreationService.Application.draft_vokis.commands.questions.update_question_content;

namespace GeneralVokiCreationService.Api.contracts.questions.update_question.update_question_content;

public class UpdateAudioOnlyQuestionContentRequest : IUpdateQuestionContentRequest
{
    public Answer[] Answers { get; init; } = [];

    public sealed record Answer(
        string Audio,
        ushort Order,
        string[] RelatedResultIds
    ) : IUpdateQuestionContentRequestAnswer;

    public ErrOrNothing Validate() {
        if (IUpdateQuestionContentRequest.ValidateAnswersCount(Answers.Length).IsErr(out var err)) {
            return err;
        }

        ErrOr<AudioOnlyUnsavedQuestionContentDto.Answer[]> answersParseRes = IUpdateQuestionContentRequest
            .ParseAnswers<AudioOnlyUnsavedQuestionContentDto.Answer, Answer>(
                Answers,
                createParsed: (answer, order, results) => {
                    if (string.IsNullOrWhiteSpace(answer.Audio)) {
                        return ErrFactory.NoValue.Common($"Audio key is required for answer {order.Value}");
                    }

                    return new AudioOnlyUnsavedQuestionContentDto.Answer(answer.Audio, order, results);
                }
            );
        if (answersParseRes.IsErr(out err)) {
            return err;
        }

        ValidatedContent = new AudioOnlyUnsavedQuestionContentDto(answersParseRes.AsSuccess());
        return ErrOrNothing.Nothing;
    }

    public BaseUnsavedQuestionContentDto ValidatedContent { get; private set; }
}