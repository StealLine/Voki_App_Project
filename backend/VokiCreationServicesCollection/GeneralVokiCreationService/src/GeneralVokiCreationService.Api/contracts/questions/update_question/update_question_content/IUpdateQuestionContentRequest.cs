using System.Text.Json.Serialization;
using GeneralVokiCreationService.Application.draft_vokis.commands.questions.update_question_content;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiCreationService.Api.contracts.questions.update_question.update_question_content;

[JsonDerivedType(typeof(UpdateTextOnlyQuestionContentRequest), typeDiscriminator: nameof(GeneralVokiQuestionContentType.TextOnly))]
[JsonDerivedType(typeof(UpdateImageOnlyQuestionContentRequest), typeDiscriminator: nameof(GeneralVokiQuestionContentType.ImageOnly))]
[JsonDerivedType(typeof(UpdateImageAndTextQuestionContentRequest), typeDiscriminator: nameof(GeneralVokiQuestionContentType.ImageAndText))]
[JsonDerivedType(typeof(UpdateColorOnlyQuestionContentRequest), typeDiscriminator: nameof(GeneralVokiQuestionContentType.ColorOnly))]
[JsonDerivedType(typeof(UpdateColorAndTextQuestionContentRequest), typeDiscriminator: nameof(GeneralVokiQuestionContentType.ColorAndText))]
[JsonDerivedType(typeof(UpdateAudioOnlyQuestionContentRequest), typeDiscriminator: nameof(GeneralVokiQuestionContentType.AudioOnly))]
[JsonDerivedType(typeof(UpdateAudioAndTextQuestionContentRequest), typeDiscriminator: nameof(GeneralVokiQuestionContentType.AudioAndText))]
public interface IUpdateQuestionContentRequest : IRequestWithValidationNeeded
{
    public BaseUnsavedQuestionContentDto ValidatedContent { get; }
    ErrOrNothing IRequestWithValidationNeeded.Validate() => ErrFactory.NotImplemented("This type is not implemented yet");

    protected static sealed ErrOrNothing ValidateAnswersCount(int answersCount) =>
        answersCount > VokiQuestion.MaxAnswersCount
            ? ErrFactory.LimitExceeded(
                $"Question cannot have more than '{VokiQuestion.MaxAnswersCount}' answers",
                $"Current count: {answersCount}"
            )
            : ErrOrNothing.Nothing;

    protected static sealed ErrOr<TParsed[]> ParseAnswers<TParsed, TInitial>(
        TInitial[] answers, Func<TInitial, AnswerOrderInQuestion, AnswerRelatedResultIdsSet, ErrOr<TParsed>> createParsed
    ) where TInitial : IUpdateQuestionContentRequestAnswer {
        List<TParsed> parsedAnswers = new(answers.Length);
        TInitial[] orderedAnswers = answers
            .OrderBy(answer => answer.Order)
            .ToArray();
        for (int i = 0; i < orderedAnswers.Length; i++) {
            int expectedOrder = i + 1;
            var currentAnswer = orderedAnswers[i];
            if (currentAnswer.Order != expectedOrder) {
                return ErrFactory.Conflict($"Incorrect answer order. Expected: {expectedOrder}. Actual: {currentAnswer.Order}");
            }

            ErrOr<AnswerOrderInQuestion> typedOrder = AnswerOrderInQuestion.Create(currentAnswer.Order);
            if (typedOrder.IsErr(out var err)) {
                return ErrFactory.IncorrectFormat(
                    $"Could not parse answer order. Expected: {typedOrder}. Actual: {currentAnswer.Order}", err.Message
                );
            }

            ImmutableHashSet<GeneralVokiResultId> resultGuids = currentAnswer.RelatedResultIds
                .Where(r => Guid.TryParse(r, out _))
                .Select(r => new GeneralVokiResultId(new(r)))
                .ToImmutableHashSet();
            ErrOr<AnswerRelatedResultIdsSet> resultsSetCreation = AnswerRelatedResultIdsSet.Create(resultGuids);
            if (resultsSetCreation.IsErr(out err)) {
                return err.WithMessagePrefix(
                    $"Could not parse related results for the answer with order: {currentAnswer.Order}. Error:"
                );
            }

            ErrOr<TParsed> parsedCreationRes = createParsed(
                currentAnswer,
                typedOrder.AsSuccess(),
                resultsSetCreation.AsSuccess()
            );
            if (parsedCreationRes.IsErr(out err)) {
                return err;
            }

            parsedAnswers.Add(parsedCreationRes.AsSuccess());
        }

        return parsedAnswers.ToArray();
    }
}

public interface IUpdateQuestionContentRequestAnswer
{
    public ushort Order { get; }
    public string[] RelatedResultIds { get; }
}