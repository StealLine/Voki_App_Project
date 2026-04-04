using System.Text.Json.Serialization;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.content_types;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiCreationService.Application.dtos;

[JsonDerivedType(typeof(TextOnlyQuestionContentPrimitiveDto), typeDiscriminator: nameof(GeneralVokiQuestionContentType.TextOnly))]
[JsonDerivedType(typeof(ImageOnlyQuestionContentPrimitiveDto), typeDiscriminator: nameof(GeneralVokiQuestionContentType.ImageOnly))]
[JsonDerivedType(typeof(ImageAndTextQuestionContentPrimitiveDto), typeDiscriminator: nameof(GeneralVokiQuestionContentType.ImageAndText))]
[JsonDerivedType(typeof(ColorOnlyQuestionContentPrimitiveDto), typeDiscriminator: nameof(GeneralVokiQuestionContentType.ColorOnly))]
[JsonDerivedType(typeof(ColorAndTextQuestionContentPrimitiveDto), typeDiscriminator: nameof(GeneralVokiQuestionContentType.ColorAndText))]
[JsonDerivedType(typeof(AudioOnlyQuestionContentPrimitiveDto), typeDiscriminator: nameof(GeneralVokiQuestionContentType.AudioOnly))]
[JsonDerivedType(typeof(AudioAndTextQuestionContentPrimitiveDto), typeDiscriminator: nameof(GeneralVokiQuestionContentType.AudioAndText))]
public interface IQuestionContentPrimitiveDto
{
    [JsonIgnore] public IReadOnlyCollection<IQuestionAnswerPrimitiveDto> BaseAnswers { get; }

    public static IQuestionContentPrimitiveDto FromQuestionContent(BaseQuestionTypeSpecificContent content) =>
        content.Match<IQuestionContentPrimitiveDto>(
            textOnly: c => new TextOnlyQuestionContentPrimitiveDto(
                c.Answers.Select(TextOnlyAnswerPrimitiveDto.FromAnswer).ToArray()
            ),
            imageOnly: c => new ImageOnlyQuestionContentPrimitiveDto(
                c.Answers.Select(ImageOnlyAnswerPrimitiveDto.FromAnswer).ToArray()
            ),
            imageAndText: c => new ImageAndTextQuestionContentPrimitiveDto(
                c.Answers.Select(ImageAndTextAnswerPrimitiveDto.FromAnswer).ToArray()
            ),
            colorOnly: c => new ColorOnlyQuestionContentPrimitiveDto(
                c.Answers.Select(ColorOnlyAnswerPrimitiveDto.FromAnswer).ToArray()
            ),
            colorAndText: c => new ColorAndTextQuestionContentPrimitiveDto(
                c.Answers.Select(ColorAndTextAnswerPrimitiveDto.FromAnswer).ToArray()
            ),
            audioOnly: c => new AudioOnlyQuestionContentPrimitiveDto(
                c.Answers.Select(AudioOnlyAnswerPrimitiveDto.FromAnswer).ToArray()
            ),
            audioAndText: c => new AudioAndTextQuestionContentPrimitiveDto(
                c.Answers.Select(AudioAndTextAnswerPrimitiveDto.FromAnswer).ToArray()
            )
        );
}

public interface IQuestionAnswerPrimitiveDto
{
    public ushort Order { get; }
    public string[] RelatedResultIds { get; }
}

public sealed record TextOnlyQuestionContentPrimitiveDto(
    TextOnlyAnswerPrimitiveDto[] Answers
) : IQuestionContentPrimitiveDto
{
    [JsonIgnore] public IReadOnlyCollection<IQuestionAnswerPrimitiveDto> BaseAnswers => Answers;
}

public sealed record TextOnlyAnswerPrimitiveDto(
    string Text,
    ushort Order,
    string[] RelatedResultIds
) : IQuestionAnswerPrimitiveDto
{
    public static TextOnlyAnswerPrimitiveDto FromAnswer(BaseQuestionAnswer.TextOnly a) => new(
        a.Text.ToString(),
        a.Order.Value,
        a.RelatedResultIds.Select(x => x.ToString()).ToArray()
    );
}

public sealed record ImageOnlyQuestionContentPrimitiveDto(
    ImageOnlyAnswerPrimitiveDto[] Answers
) : IQuestionContentPrimitiveDto
{
    [JsonIgnore] public IReadOnlyCollection<IQuestionAnswerPrimitiveDto> BaseAnswers => Answers;
}

public sealed record ImageOnlyAnswerPrimitiveDto(
    string Image,
    ushort Order,
    string[] RelatedResultIds
) : IQuestionAnswerPrimitiveDto
{
    public static ImageOnlyAnswerPrimitiveDto FromAnswer(BaseQuestionAnswer.ImageOnly a) => new(
        a.Image.ToString(),
        a.Order.Value,
        a.RelatedResultIds.Select(x => x.ToString()).ToArray()
    );
}

public sealed record ImageAndTextQuestionContentPrimitiveDto(
    ImageAndTextAnswerPrimitiveDto[] Answers
) : IQuestionContentPrimitiveDto
{
    [JsonIgnore] public IReadOnlyCollection<IQuestionAnswerPrimitiveDto> BaseAnswers => Answers;
}

public sealed record ImageAndTextAnswerPrimitiveDto(
    string Text,
    string Image,
    ushort Order,
    string[] RelatedResultIds
) : IQuestionAnswerPrimitiveDto
{
    public static ImageAndTextAnswerPrimitiveDto FromAnswer(BaseQuestionAnswer.ImageAndText a) => new(
        a.Text.ToString(),
        a.Image.ToString(),
        a.Order.Value,
        a.RelatedResultIds.Select(x => x.ToString()).ToArray()
    );
}

public sealed record ColorOnlyQuestionContentPrimitiveDto(
    ColorOnlyAnswerPrimitiveDto[] Answers
) : IQuestionContentPrimitiveDto
{
    [JsonIgnore] public IReadOnlyCollection<IQuestionAnswerPrimitiveDto> BaseAnswers => Answers;
}

public sealed record ColorOnlyAnswerPrimitiveDto(
    string Color,
    ushort Order,
    string[] RelatedResultIds
) : IQuestionAnswerPrimitiveDto
{
    public static ColorOnlyAnswerPrimitiveDto FromAnswer(BaseQuestionAnswer.ColorOnly a) => new(
        a.Color.ToString(),
        a.Order.Value,
        a.RelatedResultIds.Select(x => x.ToString()).ToArray()
    );
}

public sealed record ColorAndTextQuestionContentPrimitiveDto(
    ColorAndTextAnswerPrimitiveDto[] Answers
) : IQuestionContentPrimitiveDto
{
    [JsonIgnore] public IReadOnlyCollection<IQuestionAnswerPrimitiveDto> BaseAnswers => Answers;
}

public sealed record ColorAndTextAnswerPrimitiveDto(
    string Text,
    string Color,
    ushort Order,
    string[] RelatedResultIds
) : IQuestionAnswerPrimitiveDto
{
    public static ColorAndTextAnswerPrimitiveDto FromAnswer(BaseQuestionAnswer.ColorAndText a) => new(
        a.Text.ToString(),
        a.Color.ToString(),
        a.Order.Value,
        a.RelatedResultIds.Select(x => x.ToString()).ToArray()
    );
}

public sealed record AudioOnlyQuestionContentPrimitiveDto(
    AudioOnlyAnswerPrimitiveDto[] Answers
) : IQuestionContentPrimitiveDto
{
    [JsonIgnore] public IReadOnlyCollection<IQuestionAnswerPrimitiveDto> BaseAnswers => Answers;
}

public sealed record AudioOnlyAnswerPrimitiveDto(
    string Audio,
    ushort Order,
    string[] RelatedResultIds
) : IQuestionAnswerPrimitiveDto
{
    public static AudioOnlyAnswerPrimitiveDto FromAnswer(BaseQuestionAnswer.AudioOnly a) => new(
        a.Audio.ToString(),
        a.Order.Value,
        a.RelatedResultIds.Select(x => x.ToString()).ToArray()
    );
}

public sealed record AudioAndTextQuestionContentPrimitiveDto(
    AudioAndTextAnswerPrimitiveDto[] Answers
) : IQuestionContentPrimitiveDto
{
    [JsonIgnore] public IReadOnlyCollection<IQuestionAnswerPrimitiveDto> BaseAnswers => Answers;
}

public sealed record AudioAndTextAnswerPrimitiveDto(
    string Text,
    string Audio,
    ushort Order,
    string[] RelatedResultIds
) : IQuestionAnswerPrimitiveDto
{
    public static AudioAndTextAnswerPrimitiveDto FromAnswer(BaseQuestionAnswer.AudioAndText a) => new(
        a.Text.ToString(),
        a.Audio.ToString(),
        a.Order.Value,
        a.RelatedResultIds.Select(x => x.ToString()).ToArray()
    );
}