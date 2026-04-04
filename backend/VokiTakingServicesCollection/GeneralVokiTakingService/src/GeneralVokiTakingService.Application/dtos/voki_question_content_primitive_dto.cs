using System.Text.Json.Serialization;
using GeneralVokiTakingService.Domain.general_voki_aggregate.questions.answers;
using GeneralVokiTakingService.Domain.general_voki_aggregate.questions.content;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiTakingService.Application.dtos;

[JsonDerivedType(typeof(TextOnlyQuestionContentPrimitiveDto), typeDiscriminator: nameof(GeneralVokiQuestionContentType.TextOnly))]
[JsonDerivedType(typeof(ImageOnlyQuestionContentPrimitiveDto), typeDiscriminator: nameof(GeneralVokiQuestionContentType.ImageOnly))]
[JsonDerivedType(typeof(ImageAndTextQuestionContentPrimitiveDto), typeDiscriminator: nameof(GeneralVokiQuestionContentType.ImageAndText))]
[JsonDerivedType(typeof(ColorOnlyQuestionContentPrimitiveDto), typeDiscriminator: nameof(GeneralVokiQuestionContentType.ColorOnly))]
[JsonDerivedType(typeof(ColorAndTextQuestionContentPrimitiveDto), typeDiscriminator: nameof(GeneralVokiQuestionContentType.ColorAndText))]
[JsonDerivedType(typeof(AudioOnlyQuestionContentPrimitiveDto), typeDiscriminator: nameof(GeneralVokiQuestionContentType.AudioOnly))]
[JsonDerivedType(typeof(AudioAndTextQuestionContentPrimitiveDto), typeDiscriminator: nameof(GeneralVokiQuestionContentType.AudioAndText))]
public interface IQuestionContentPrimitiveDto
{
    public static IQuestionContentPrimitiveDto FromQuestionContent(GeneralVokiQuestionContent content) =>
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
    public string Id { get; }

    public ushort Order { get; }
    public string[] RelatedResultIds { get; }
}

public sealed record TextOnlyQuestionContentPrimitiveDto(
    TextOnlyAnswerPrimitiveDto[] Answers
) : IQuestionContentPrimitiveDto;
public sealed record TextOnlyAnswerPrimitiveDto(
    string Text,
    string Id,
    ushort Order,
    string[] RelatedResultIds
) : IQuestionAnswerPrimitiveDto
{
    public static TextOnlyAnswerPrimitiveDto FromAnswer(BaseQuestionAnswer.TextOnly a) => new(
        a.Text.ToString(),
        a.Id.ToString(),
        a.Order,
        a.RelatedResultIds.Select(x => x.ToString()).ToArray()
    );
}

public sealed record ImageOnlyQuestionContentPrimitiveDto(
    ImageOnlyAnswerPrimitiveDto[] Answers
) : IQuestionContentPrimitiveDto;

public sealed record ImageOnlyAnswerPrimitiveDto(
    string Image,
    string Id,
    ushort Order,
    string[] RelatedResultIds
) : IQuestionAnswerPrimitiveDto
{
    public static ImageOnlyAnswerPrimitiveDto FromAnswer(BaseQuestionAnswer.ImageOnly a) => new(
        a.Image.ToString(),
        a.Id.ToString(),
        a.Order,
        a.RelatedResultIds.Select(x => x.ToString()).ToArray()
    );
}

public sealed record ImageAndTextQuestionContentPrimitiveDto(
    ImageAndTextAnswerPrimitiveDto[] Answers
) : IQuestionContentPrimitiveDto;

public sealed record ImageAndTextAnswerPrimitiveDto(
    string Text,
    string Image,
    string Id,
    ushort Order,
    string[] RelatedResultIds
) : IQuestionAnswerPrimitiveDto
{
    public static ImageAndTextAnswerPrimitiveDto FromAnswer(BaseQuestionAnswer.ImageAndText a) => new(
        a.Text.ToString(),
        a.Image.ToString(),
        a.Id.ToString(),
        a.Order,
        a.RelatedResultIds.Select(x => x.ToString()).ToArray()
    );
}

public sealed record ColorOnlyQuestionContentPrimitiveDto(
    ColorOnlyAnswerPrimitiveDto[] Answers
) : IQuestionContentPrimitiveDto;
public sealed record ColorOnlyAnswerPrimitiveDto(
    string Color,
    string Id,
    ushort Order,
    string[] RelatedResultIds
) : IQuestionAnswerPrimitiveDto
{
    public static ColorOnlyAnswerPrimitiveDto FromAnswer(BaseQuestionAnswer.ColorOnly a) => new(
        a.Color.ToString(),
        a.Id.ToString(),
        a.Order,
        a.RelatedResultIds.Select(x => x.ToString()).ToArray()
    );
}

public sealed record ColorAndTextQuestionContentPrimitiveDto(
    ColorAndTextAnswerPrimitiveDto[] Answers
) : IQuestionContentPrimitiveDto;

public sealed record ColorAndTextAnswerPrimitiveDto(
    string Text,
    string Color,
    string Id,
    ushort Order,
    string[] RelatedResultIds
) : IQuestionAnswerPrimitiveDto
{
    public static ColorAndTextAnswerPrimitiveDto FromAnswer(BaseQuestionAnswer.ColorAndText a) => new(
        a.Text.ToString(),
        a.Color.ToString(),
        a.Id.ToString(),
        a.Order,
        a.RelatedResultIds.Select(x => x.ToString()).ToArray()
    );
}

public sealed record AudioOnlyQuestionContentPrimitiveDto(
    AudioOnlyAnswerPrimitiveDto[] Answers
) : IQuestionContentPrimitiveDto;

public sealed record AudioOnlyAnswerPrimitiveDto(
    string Audio,
    string Id,
    ushort Order,
    string[] RelatedResultIds
) : IQuestionAnswerPrimitiveDto
{
    public static AudioOnlyAnswerPrimitiveDto FromAnswer(BaseQuestionAnswer.AudioOnly a) => new(
        a.Audio.ToString(),
        a.Id.ToString(),
        a.Order,
        a.RelatedResultIds.Select(x => x.ToString()).ToArray()
    );
}

public sealed record AudioAndTextQuestionContentPrimitiveDto(
    AudioAndTextAnswerPrimitiveDto[] Answers
) : IQuestionContentPrimitiveDto;

public sealed record AudioAndTextAnswerPrimitiveDto(
    string Text,
    string Audio,
    string Id,
    ushort Order,
    string[] RelatedResultIds
) : IQuestionAnswerPrimitiveDto
{
    public static AudioAndTextAnswerPrimitiveDto FromAnswer(BaseQuestionAnswer.AudioAndText a) => new(
        a.Text.ToString(),
        a.Audio.ToString(),
        a.Id.ToString(),
        a.Order,
        a.RelatedResultIds.Select(x => x.ToString()).ToArray()
    );
}