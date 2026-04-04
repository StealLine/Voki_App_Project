using System.Runtime.CompilerServices;
using System.Text.Json;
using GeneralVokiTakingService.Application.dtos;
using GeneralVokiTakingService.Domain.general_voki_aggregate.questions.answers;
using GeneralVokiTakingService.Domain.general_voki_aggregate.questions.content;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.value_converters.vokis;

public sealed class VokiQuestionTypeSpecificContentConverter : ValueConverter<GeneralVokiQuestionContent, string>
{
    public VokiQuestionTypeSpecificContentConverter()
        : base(
            v => ToString(v),
            s => FromString(s)
        ) { }


    private static readonly JsonSerializerOptions JsonOpts = new() {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = false
    };

    private static string ToString(GeneralVokiQuestionContent content) =>
        JsonSerializer.Serialize(IQuestionContentPrimitiveDto.FromQuestionContent(content), JsonOpts);

    private static GeneralVokiQuestionContent FromString(string json) =>
        JsonSerializer.Deserialize<IQuestionContentPrimitiveDto>(json, JsonOpts) switch {
            null => throw new ArgumentNullException(nameof(json)),
            TextOnlyQuestionContentPrimitiveDto t => CreateTextOnly(t),
            ImageOnlyQuestionContentPrimitiveDto t => CreateImageOnly(t),
            ImageAndTextQuestionContentPrimitiveDto t => CreateImageAndText(t),
            ColorOnlyQuestionContentPrimitiveDto t => CreateColorOnly(t),
            ColorAndTextQuestionContentPrimitiveDto t => CreateColorAndText(t),
            AudioOnlyQuestionContentPrimitiveDto t => CreateAudioOnly(t),
            AudioAndTextQuestionContentPrimitiveDto t => CreateAudioAndText(t),
            _ => throw new SwitchExpressionException(json)
        };

    private static GeneralVokiQuestionContent CreateTextOnly(TextOnlyQuestionContentPrimitiveDto t) =>
        new GeneralVokiQuestionContent.TextOnly(
            CreateAnswers(t.Answers, a => new BaseQuestionAnswer.TextOnly(
                GeneralVokiAnswerText.Create(a.Text).AsSuccess(),
                new(new(a.Id)),
                a.Order,
                CreateRelated(a.RelatedResultIds)
            ))
        );

    private static GeneralVokiQuestionContent CreateImageOnly(ImageOnlyQuestionContentPrimitiveDto t) =>
        new GeneralVokiQuestionContent.ImageOnly(
            CreateAnswers(t.Answers, a => new BaseQuestionAnswer.ImageOnly(
                new GeneralVokiAnswerImageKey(a.Image),
                new(new(a.Id)),
                a.Order,
                CreateRelated(a.RelatedResultIds)
            ))
        );

    private static GeneralVokiQuestionContent CreateImageAndText(ImageAndTextQuestionContentPrimitiveDto t) =>
        new GeneralVokiQuestionContent.ImageAndText(
            CreateAnswers(t.Answers, a => new BaseQuestionAnswer.ImageAndText(
                new GeneralVokiAnswerImageKey(a.Image),
                GeneralVokiAnswerText.Create(a.Text).AsSuccess(),
                new(new(a.Id)),
                a.Order,
                CreateRelated(a.RelatedResultIds)
            ))
        );

    private static GeneralVokiQuestionContent CreateColorOnly(ColorOnlyQuestionContentPrimitiveDto t) =>
        new GeneralVokiQuestionContent.ColorOnly(
            CreateAnswers(t.Answers, a => new BaseQuestionAnswer.ColorOnly(
                new HexColor(a.Color),
                new(new(a.Id)),
                a.Order,
                CreateRelated(a.RelatedResultIds)
            ))
        );

    private static GeneralVokiQuestionContent CreateColorAndText(ColorAndTextQuestionContentPrimitiveDto t) =>
        new GeneralVokiQuestionContent.ColorAndText(
            CreateAnswers(t.Answers, a => new BaseQuestionAnswer.ColorAndText(
                new HexColor(a.Color),
                GeneralVokiAnswerText.Create(a.Text).AsSuccess(),
                new(new(a.Id)),
                a.Order,
                CreateRelated(a.RelatedResultIds)
            ))
        );

    private static GeneralVokiQuestionContent CreateAudioOnly(AudioOnlyQuestionContentPrimitiveDto t) =>
        new GeneralVokiQuestionContent.AudioOnly(
            CreateAnswers(t.Answers, a => new BaseQuestionAnswer.AudioOnly(
                new GeneralVokiAnswerAudioKey(a.Audio),
                new(new(a.Id)),
                a.Order,
                CreateRelated(a.RelatedResultIds)
            ))
        );

    private static GeneralVokiQuestionContent CreateAudioAndText(AudioAndTextQuestionContentPrimitiveDto t) =>
        new GeneralVokiQuestionContent.AudioAndText(
            CreateAnswers(t.Answers, a => new BaseQuestionAnswer.AudioAndText(
                new GeneralVokiAnswerAudioKey(a.Audio),
                GeneralVokiAnswerText.Create(a.Text).AsSuccess(),
                new(new(a.Id)),
                a.Order,
                CreateRelated(a.RelatedResultIds)
            ))
        );


    private static ImmutableArray<TOut> CreateAnswers<TIn, TOut>(
        TIn[] source, Func<TIn, TOut> func
    ) where TOut : BaseQuestionAnswer =>
        source.Select(func).ToImmutableArray();

    private static ImmutableHashSet<GeneralVokiResultId> CreateRelated(string[] ids) =>
        ids.Select(id => new GeneralVokiResultId(new Guid(id))).ToImmutableHashSet();
}