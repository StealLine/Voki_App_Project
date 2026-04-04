using System.Net.Mime;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.content_types;
using SharedKernel.common.vokis.general_vokis;
using VokiCreationServicesLib.Domain.draft_voki_aggregate.publishing;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

public class VokiQuestion : Entity<GeneralVokiQuestionId>
{
    public const int
        MinAnswersCount = 2,
        MaxAnswersCount = 60;

    private VokiQuestion() { }
    public VokiQuestionText Text { get; private set; }
    public VokiQuestionImagesSet ImageSet { get; private set; }
    public VokiQuestionOrder OrderInVoki { get; private set; }
    public BaseQuestionTypeSpecificContent Content { get; private set; }
    public bool ShuffleAnswers { get; private set; }
    public QuestionAnswersCountLimit AnswersCountLimit { get; private set; }

    private VokiQuestion(
        GeneralVokiQuestionId id,
        VokiQuestionText text,
        VokiQuestionImagesSet imageSet,
        VokiQuestionOrder orderInVoki,
        GeneralVokiQuestionContentType contentType,
        QuestionAnswersCountLimit answersCountLimit
    ) {
        Id = id;
        Text = text;
        ImageSet = imageSet;
        Content = BaseQuestionTypeSpecificContent.CreateEmpty(contentType);
        OrderInVoki = orderInVoki;
        AnswersCountLimit = answersCountLimit;
        ShuffleAnswers = false;
    }

    public static VokiQuestion CreateNew(VokiQuestionOrder orderInVoki, GeneralVokiQuestionContentType contentType) => new(
        GeneralVokiQuestionId.CreateNew(),
        GeneralVokiPresets.GetRandomQuestionText(),
        VokiQuestionImagesSet.Default,
        orderInVoki,
        contentType,
        QuestionAnswersCountLimit.SingleChoice()
    );

    public bool HasAudio() {
        return Content.Type == GeneralVokiQuestionContentType.AudioOnly
               || Content.Type == GeneralVokiQuestionContentType.AudioAndText;
    }

    public void UpdateText(VokiQuestionText questionText) {
        Text = questionText;
    }

    public ErrOrNothing MoveOrderUp() {
        ErrOr<VokiQuestionOrder> newOrder = OrderInVoki.MinusOne();
        if (newOrder.IsErr(out var err)) {
            return err;
        }

        OrderInVoki = newOrder.AsSuccess();
        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing MoveOrderDown() {
        ErrOr<VokiQuestionOrder> newOrder = OrderInVoki.PlusOne();
        if (newOrder.IsErr(out var err)) {
            return err;
        }

        OrderInVoki = newOrder.AsSuccess();
        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing UpdateImages(VokiQuestionImagesSet images) {
        if (images.Keys.Any(k => k.QuestionId != Id)) {
            return ErrFactory.Conflict("One or more images does not belong to this question");
        }

        ImageSet = images;
        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing UpdateAnswerSettings(QuestionAnswersCountLimit newCountLimit, bool shuffleAnswers) {
        if (newCountLimit.MaxAnswers > MaxAnswersCount) {
            return ErrFactory.LimitExceeded($"Maximum answers count cannot be greater than {MaxAnswersCount}");
        }

        AnswersCountLimit = newCountLimit;
        ShuffleAnswers = shuffleAnswers;
        return ErrOrNothing.Nothing;
    }

    public void RemoveRelatedResultInAnswers(GeneralVokiResultId resultId) =>
        Content = Content.RemoveResult(resultId);

    public List<VokiPublishingIssue> CheckForPublishingIssues() {
        string questionText = Text.ToString();
        string preview = questionText.Length > 15
            ? questionText[..15] + "..."
            : questionText;
        var answersCount = Content.BaseAnswers.Count();
        if (answersCount < MinAnswersCount) {
            return [
                VokiPublishingIssue.Problem(
                    message:
                    $"[\"{preview}\"] question has too few answers ({answersCount}). Minimum required is {MinAnswersCount}",
                    source: "Question answers",
                    fixRecommendation: $"Add at least {MinAnswersCount - answersCount} more answer(s)"
                )
            ];
        }

        if (answersCount > MaxAnswersCount) {
            return [
                VokiPublishingIssue.Problem(
                    message:
                    $"[\"{preview}\"] question has too many answers ({answersCount}). Maximum allowed is {MaxAnswersCount}",
                    source: "Question answers",
                    fixRecommendation: $"Remove {answersCount - MaxAnswersCount} answer(s) to meet the limit"
                )
            ];
        }

        if (answersCount < AnswersCountLimit.MinAnswers) {
            return [
                VokiPublishingIssue.Problem(
                    message:
                    $"[\"{preview}\"] question's answer count is below the configured minimum ({AnswersCountLimit.MinAnswers})",
                    source: "Question answers",
                    fixRecommendation:
                    $"Decrease the minimum limit or add at least {AnswersCountLimit.MinAnswers - answersCount} more answer(s)"
                )
            ];
        }

        if (answersCount < AnswersCountLimit.MaxAnswers) {
            return [
                VokiPublishingIssue.Problem(
                    message:
                    $"[\"{preview}\"] question's answer count is below the configured maximum({AnswersCountLimit.MaxAnswers})",
                    source: "Question answers",
                    fixRecommendation:
                    $"Decrease the maximum limit or add at least {AnswersCountLimit.MaxAnswers - answersCount} answer(s)"
                )
            ];
        }

        if (
            AnswersCountLimit.MinAnswers == AnswersCountLimit.MaxAnswers
            && answersCount == AnswersCountLimit.MinAnswers
        ) {
            return [
                VokiPublishingIssue.Problem(
                    message:
                    $"[\"{preview}\"] question's minimum and maximum answer limits are equal ({AnswersCountLimit.MinAnswers}), and the question has exactly that number of answers, making the question meaningless",
                    source: "Question answers",
                    fixRecommendation: "Adjust the minimum and maximum answers count limit or add new another answer"
                )
            ];
        }

        return [];
    }

    public void UpdateContent(BaseQuestionTypeSpecificContent newContent) =>
        this.Content = newContent;
}