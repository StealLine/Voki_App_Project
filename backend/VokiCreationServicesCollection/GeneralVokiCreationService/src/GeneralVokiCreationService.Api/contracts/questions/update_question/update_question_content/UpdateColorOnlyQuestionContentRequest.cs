using GeneralVokiCreationService.Application.draft_vokis.commands.questions.update_question_content;
using SharedKernel.common;

namespace GeneralVokiCreationService.Api.contracts.questions.update_question.update_question_content;

public class UpdateColorOnlyQuestionContentRequest : IUpdateQuestionContentRequest
{
    public Answer[] Answers { get; init; } = [];

    public sealed record Answer(
        string Color,
        ushort Order,
        string[] RelatedResultIds
    ) : IUpdateQuestionContentRequestAnswer;

    public ErrOrNothing Validate()
    {
        if (IUpdateQuestionContentRequest.ValidateAnswersCount(Answers.Length).IsErr(out var err))
        {
            return err;
        }

        ErrOr<ColorOnlyUnsavedQuestionContentDto.Answer[]> answersParseRes = IUpdateQuestionContentRequest
            .ParseAnswers<ColorOnlyUnsavedQuestionContentDto.Answer, Answer>(
                Answers,
                createParsed: (answer, order, results) => HexColor.Create(answer.Color)
                    .Match<ErrOr<ColorOnlyUnsavedQuestionContentDto.Answer>>(
                        successFunc: (color) => new ColorOnlyUnsavedQuestionContentDto.Answer(color, order, results),
                        errorFunc: (e) => e.WithMessagePrefix($"Error in the answer with order: {order}. Error:")
                    )
            );
        if (answersParseRes.IsErr(out err))
        {
            return err;
        }

        ValidatedContent = new ColorOnlyUnsavedQuestionContentDto(answersParseRes.AsSuccess());
        return ErrOrNothing.Nothing;
    }

    public BaseUnsavedQuestionContentDto ValidatedContent { get; private set; }
}
