using System.Collections.Immutable;
using System.Text.Json;
using GeneralVokiCreationService.Application.dtos;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.content_types;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.common.vokis.general_vokis;
using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiCreationService.Infrastructure.persistence.configurations.value_converters.questions;

public sealed class VokiQuestionTypeSpecificContentConverter
    : ValueConverter<BaseQuestionTypeSpecificContent, string>
{
    public VokiQuestionTypeSpecificContentConverter()
        : base(
            v => ToString(v),
            s => FromString(s)
        ) { }

    private const string TypeAndJsonDivider = ":";

    private static readonly JsonSerializerOptions JsonOpts = new() {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = false
    };

    private static string ToString(BaseQuestionTypeSpecificContent content) {
        var type = content.Type;
        var json = JsonSerializer.Serialize(IQuestionContentPrimitiveDto.FromQuestionContent(content), JsonOpts);
        return $"{type}{TypeAndJsonDivider}{json}";
    }

    private static BaseQuestionTypeSpecificContent FromString(string value) {
        var parts = value.Split(TypeAndJsonDivider, 2);
        var type = Enum.Parse<GeneralVokiQuestionContentType>(parts[0]);
        var json = parts[1];
        return FromDbStore(type, json);
    }


    private static BaseQuestionTypeSpecificContent FromDbStore(
        GeneralVokiQuestionContentType type,
        string json
    ) => type.Match<BaseQuestionTypeSpecificContent>(
        textOnly: () => {
            var dto = JsonSerializer.Deserialize<TextOnlyQuestionContentPrimitiveDto>(json, JsonOpts)!;
            return new BaseQuestionTypeSpecificContent.TextOnly(
                CreateAnswers(dto.Answers, a =>
                    new BaseQuestionAnswer.TextOnly(
                        GeneralVokiAnswerText.Create(a.Text).AsSuccess(),
                        CreateOrder(a.Order),
                        CreateRelated(a.RelatedResultIds)
                    )
                )
            );
        },
        imageOnly: () => {
            var dto = JsonSerializer.Deserialize<ImageOnlyQuestionContentPrimitiveDto>(json, JsonOpts)!;
            return new BaseQuestionTypeSpecificContent.ImageOnly(
                CreateAnswers(dto.Answers, a =>
                    new BaseQuestionAnswer.ImageOnly(
                        new GeneralVokiAnswerImageKey(a.Image),
                        CreateOrder(a.Order),
                        CreateRelated(a.RelatedResultIds)
                    )
                )
            );
        },
        imageAndText: () => {
            var dto = JsonSerializer.Deserialize<ImageAndTextQuestionContentPrimitiveDto>(json, JsonOpts)!;
            return new BaseQuestionTypeSpecificContent.ImageAndText(
                CreateAnswers(dto.Answers, a =>
                    new BaseQuestionAnswer.ImageAndText(
                        GeneralVokiAnswerText.Create(a.Text).AsSuccess(),
                        new GeneralVokiAnswerImageKey(a.Image),
                        CreateOrder(a.Order),
                        CreateRelated(a.RelatedResultIds)
                    )
                )
            );
        },
        colorOnly: () => {
            var dto = JsonSerializer.Deserialize<ColorOnlyQuestionContentPrimitiveDto>(json, JsonOpts)!;
            return new BaseQuestionTypeSpecificContent.ColorOnly(
                CreateAnswers(dto.Answers, a =>
                    new BaseQuestionAnswer.ColorOnly(
                        HexColor.Create(a.Color).AsSuccess(),
                        CreateOrder(a.Order),
                        CreateRelated(a.RelatedResultIds)
                    )
                )
            );
        },
        colorAndText: () => {
            var dto = JsonSerializer.Deserialize<ColorAndTextQuestionContentPrimitiveDto>(json, JsonOpts)!;
            return new BaseQuestionTypeSpecificContent.ColorAndText(
                CreateAnswers(dto.Answers, a =>
                    new BaseQuestionAnswer.ColorAndText(
                        GeneralVokiAnswerText.Create(a.Text).AsSuccess(),
                        HexColor.Create(a.Color).AsSuccess(),
                        CreateOrder(a.Order),
                        CreateRelated(a.RelatedResultIds)
                    )
                )
            );
        },
        audioOnly: () => {
            var dto = JsonSerializer.Deserialize<AudioOnlyQuestionContentPrimitiveDto>(json, JsonOpts)!;
            return new BaseQuestionTypeSpecificContent.AudioOnly(
                CreateAnswers(dto.Answers, a =>
                    new BaseQuestionAnswer.AudioOnly(
                        new GeneralVokiAnswerAudioKey(a.Audio),
                        CreateOrder(a.Order),
                        CreateRelated(a.RelatedResultIds)
                    )
                )
            );
        },
        audioAndText: () => {
            var dto = JsonSerializer.Deserialize<AudioAndTextQuestionContentPrimitiveDto>(json, JsonOpts)!;
            return new BaseQuestionTypeSpecificContent.AudioAndText(
                CreateAnswers(dto.Answers, a =>
                    new BaseQuestionAnswer.AudioAndText(
                        GeneralVokiAnswerText.Create(a.Text).AsSuccess(),
                        new GeneralVokiAnswerAudioKey(a.Audio),
                        CreateOrder(a.Order),
                        CreateRelated(a.RelatedResultIds)
                    )
                )
            );
        }
    );


    private static QuestionAnswersList<TOut> CreateAnswers<TIn, TOut>(
        TIn[] source, Func<TIn, TOut> func
    ) where TOut : BaseQuestionAnswer =>
        QuestionAnswersList<TOut>.Create(answers: source.Select(func)).AsSuccess();

    private static AnswerOrderInQuestion CreateOrder(ushort order) =>
        AnswerOrderInQuestion.Create(order).AsSuccess();

    private static AnswerRelatedResultIdsSet CreateRelated(string[] ids) =>
        AnswerRelatedResultIdsSet.Create(
            ids.Select(id => new GeneralVokiResultId(new Guid(id))).ToImmutableHashSet()
        ).AsSuccess();
}