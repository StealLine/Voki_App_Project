using System.Runtime.CompilerServices;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;
using SharedKernel.common.vokis.general_vokis;
using SharedKernel.integration_events.voki_publishing;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.content_types;

public abstract partial record BaseQuestionTypeSpecificContent
{
    public abstract GeneralVokiQuestionContentType Type { get; }
    public abstract IEnumerable<BaseQuestionAnswer> BaseAnswers { get; }

    [Pure]
    public abstract BaseQuestionTypeSpecificContent RemoveResult(GeneralVokiResultId resultId);

    public TResult Match<TResult>(
        Func<TextOnly, TResult> textOnly,
        Func<ImageOnly, TResult> imageOnly,
        Func<ImageAndText, TResult> imageAndText,
        Func<ColorOnly, TResult> colorOnly,
        Func<ColorAndText, TResult> colorAndText,
        Func<AudioOnly, TResult> audioOnly,
        Func<AudioAndText, TResult> audioAndText
    ) => this switch
    {
        TextOnly typed => textOnly(typed),
        ImageOnly typed => imageOnly(typed),
        ImageAndText typed => imageAndText(typed),
        ColorOnly typed => colorOnly(typed),
        ColorAndText typed => colorAndText(typed),
        AudioOnly typed => audioOnly(typed),
        AudioAndText typed => audioAndText(typed),
        _ => throw new SwitchExpressionException(this)
    };

    public static BaseQuestionTypeSpecificContent CreateEmpty(GeneralVokiQuestionContentType t) =>
        t.Match<BaseQuestionTypeSpecificContent>(
            textOnly: TextOnly.Empty,
            imageOnly: ImageOnly.Empty,
            imageAndText: ImageAndText.Empty,
            colorOnly: ColorOnly.Empty,
            colorAndText: ColorAndText.Empty,
            audioOnly: AudioOnly.Empty,
            audioAndText: AudioAndText.Empty
        );
    public abstract IQuestionContentIntegrationEventDto ToIntegrationEventDto();
}

public interface IContentWithStorageKeys
{
    public bool IsAllForCorrectVokiQuestion(VokiId vokiId, GeneralVokiQuestionId questionId);
}