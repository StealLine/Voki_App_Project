using System.Runtime.CompilerServices;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.content_types;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.questions.update_question_content;

public abstract record BaseUnsavedQuestionContentDto()
{
    public TResult Match<TResult>(
        Func<TextOnlyUnsavedQuestionContentDto, TResult> textOnly,
        Func<ImageOnlyUnsavedQuestionContentDto, TResult> imageOnly,
        Func<ImageAndTextUnsavedQuestionContentDto, TResult> imageAndText,
        Func<ColorOnlyUnsavedQuestionContentDto, TResult> colorOnly,
        Func<ColorAndTextUnsavedQuestionContentDto, TResult> colorAndText,
        Func<AudioOnlyUnsavedQuestionContentDto, TResult> audioOnly,
        Func<AudioAndTextUnsavedQuestionContentDto, TResult> audioAndText
    ) => this switch {
        TextOnlyUnsavedQuestionContentDto typed => textOnly(typed),
        ImageOnlyUnsavedQuestionContentDto typed => imageOnly(typed),
        ImageAndTextUnsavedQuestionContentDto typed => imageAndText(typed),
        ColorOnlyUnsavedQuestionContentDto typed => colorOnly(typed),
        ColorAndTextUnsavedQuestionContentDto typed => colorAndText(typed),
        AudioOnlyUnsavedQuestionContentDto typed => audioOnly(typed),
        AudioAndTextUnsavedQuestionContentDto typed => audioAndText(typed),
        _ => throw new SwitchExpressionException(this)
    };
}

public sealed record TextOnlyUnsavedQuestionContentDto(
    TextOnlyUnsavedQuestionContentDto.Answer[] Answers
) : BaseUnsavedQuestionContentDto
{
    public sealed record Answer(
        GeneralVokiAnswerText Text,
        AnswerOrderInQuestion Order,
        AnswerRelatedResultIdsSet RelatedResultIds
    );

    public ErrOr<BaseQuestionTypeSpecificContent.TextOnly> ToSavedContent() =>
        QuestionAnswersList<BaseQuestionAnswer.TextOnly>
            .Create(Answers.Select(a => new BaseQuestionAnswer.TextOnly(a.Text, a.Order, a.RelatedResultIds)))
            .Bind<BaseQuestionTypeSpecificContent.TextOnly>(answersList =>
                new BaseQuestionTypeSpecificContent.TextOnly(answersList)
            );
}

public record ImageOnlyUnsavedQuestionContentDto(
    ImageOnlyUnsavedQuestionContentDto.Answer[] Answers
) : BaseUnsavedQuestionContentDto
{
    public sealed record Answer(
        string ImageKey,
        AnswerOrderInQuestion Order,
        AnswerRelatedResultIdsSet RelatedResultIds
    );
}

public record ImageAndTextUnsavedQuestionContentDto(
    ImageAndTextUnsavedQuestionContentDto.Answer[] Answers
) : BaseUnsavedQuestionContentDto
{
    public sealed record Answer(
        GeneralVokiAnswerText Text,
        string ImageKey,
        AnswerOrderInQuestion Order,
        AnswerRelatedResultIdsSet RelatedResultIds
    );
}

public record ColorOnlyUnsavedQuestionContentDto(
    ColorOnlyUnsavedQuestionContentDto.Answer[] Answers
) : BaseUnsavedQuestionContentDto
{
    public sealed record Answer(
        HexColor Color,
        AnswerOrderInQuestion Order,
        AnswerRelatedResultIdsSet RelatedResultIds
    );

    public ErrOr<BaseQuestionTypeSpecificContent.ColorOnly> ToSavedContent() =>
        QuestionAnswersList<BaseQuestionAnswer.ColorOnly>
            .Create(Answers.Select(a => new BaseQuestionAnswer.ColorOnly(a.Color, a.Order, a.RelatedResultIds)))
            .Bind<BaseQuestionTypeSpecificContent.ColorOnly>(answersList =>
                new BaseQuestionTypeSpecificContent.ColorOnly(answersList)
            );
}

public record ColorAndTextUnsavedQuestionContentDto(
    ColorAndTextUnsavedQuestionContentDto.Answer[] Answers
) : BaseUnsavedQuestionContentDto
{
    public sealed record Answer(
        GeneralVokiAnswerText Text,
        HexColor Color,
        AnswerOrderInQuestion Order,
        AnswerRelatedResultIdsSet RelatedResultIds
    );

    public ErrOr<BaseQuestionTypeSpecificContent.ColorAndText> ToSavedContent() =>
        QuestionAnswersList<BaseQuestionAnswer.ColorAndText>
            .Create(Answers.Select(a => new BaseQuestionAnswer.ColorAndText(a.Text, a.Color, a.Order, a.RelatedResultIds)))
            .Bind<BaseQuestionTypeSpecificContent.ColorAndText>(answersList =>
                new BaseQuestionTypeSpecificContent.ColorAndText(answersList)
            );
}

public record AudioOnlyUnsavedQuestionContentDto(
    AudioOnlyUnsavedQuestionContentDto.Answer[] Answers
) : BaseUnsavedQuestionContentDto
{
    public sealed record Answer(
        string AudioKey,
        AnswerOrderInQuestion Order,
        AnswerRelatedResultIdsSet RelatedResultIds
    );
}

public record AudioAndTextUnsavedQuestionContentDto(
    AudioAndTextUnsavedQuestionContentDto.Answer[] Answers
) : BaseUnsavedQuestionContentDto
{
    public sealed record Answer(
        GeneralVokiAnswerText Text,
        string AudioKey,
        AnswerOrderInQuestion Order,
        AnswerRelatedResultIdsSet RelatedResultIds
    );
}