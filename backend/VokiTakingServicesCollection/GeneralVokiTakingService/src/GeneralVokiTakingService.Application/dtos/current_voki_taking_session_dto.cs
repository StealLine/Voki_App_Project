using System.Text.Json.Serialization;
using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.general_voki_aggregate.questions;
using GeneralVokiTakingService.Domain.general_voki_aggregate.questions.answers;
using GeneralVokiTakingService.Domain.general_voki_aggregate.questions.content;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using SharedKernel.common.vokis;
using SharedKernel.common.vokis.general_vokis;
using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiTakingService.Application.dtos;

public record CurrentVokiTakingSessionDto(
    VokiId VokiId,
    VokiName VokiName,
    bool IsWithForceSequentialAnswering,
    VokiTakingQuestionData[] QuestionsToShow,
    VokiTakingSessionId SessionId,
    DateTime StartedAt,
    ushort TotalQuestionsCount
)
{
    public static CurrentVokiTakingSessionDto Create(
        VokiName vokiName,
        IDictionary<GeneralVokiQuestionId, VokiQuestion> vokiQuestionsById,
        BaseVokiTakingSession takingSession,
        IEnumerable<TakingSessionExpectedQuestion> questionsToShow
    ) {
        VokiTakingQuestionData[] questions = questionsToShow
            .Select((q) => VokiTakingQuestionData.Create(
                vokiQuestionsById[q.QuestionId],
                q.OrderInVokiTaking,
                q.AnswersIdToOrderInQuestion
            ))
            .ToArray();
        return new CurrentVokiTakingSessionDto(
            takingSession.VokiId, vokiName, takingSession.IsWithForceSequentialAnswering, questions,
            takingSession.Id, takingSession.StartTime, takingSession.TotalQuestionsCount
        );
    }
}

public record VokiTakingQuestionData(
    GeneralVokiQuestionId Id,
    string Text,
    VokiQuestionImagesSet ImagesSet,
    IVokiTakingQuestionContentPrimitiveDto Content,
    QuestionOrderInVokiTakingSession OrderInVokiTaking,
    ushort MinAnswersCount,
    ushort MaxAnswersCount
)
{
    public static VokiTakingQuestionData Create(
        VokiQuestion question,
        QuestionOrderInVokiTakingSession orderInVokiTaking,
        ImmutableDictionary<GeneralVokiAnswerId, ushort> answersIdToOrderInQuestion
    ) => new(
        question.Id,
        question.Text,
        question.ImageSet,
        IVokiTakingQuestionContentPrimitiveDto.Create(question.Content, answersIdToOrderInQuestion),
        orderInVokiTaking,
        question.AnswersCountLimit.MinAnswers,
        question.AnswersCountLimit.MaxAnswers
    );
}

[JsonDerivedType(typeof(TextOnly), typeDiscriminator: nameof(GeneralVokiQuestionContentType.TextOnly))]
[JsonDerivedType(typeof(ImageOnly), typeDiscriminator: nameof(GeneralVokiQuestionContentType.ImageOnly))]
[JsonDerivedType(typeof(ImageAndText), typeDiscriminator: nameof(GeneralVokiQuestionContentType.ImageAndText))]
[JsonDerivedType(typeof(ColorOnly), typeDiscriminator: nameof(GeneralVokiQuestionContentType.ColorOnly))]
[JsonDerivedType(typeof(ColorAndText), typeDiscriminator: nameof(GeneralVokiQuestionContentType.ColorAndText))]
[JsonDerivedType(typeof(AudioOnly), typeDiscriminator: nameof(GeneralVokiQuestionContentType.AudioOnly))]
[JsonDerivedType(typeof(AudioAndText), typeDiscriminator: nameof(GeneralVokiQuestionContentType.AudioAndText))]
public interface IVokiTakingQuestionContentPrimitiveDto
{
    public static IVokiTakingQuestionContentPrimitiveDto Create(
        GeneralVokiQuestionContent content,
        IDictionary<GeneralVokiAnswerId, ushort> answersIdToOrder
    ) =>
        content.Match<IVokiTakingQuestionContentPrimitiveDto>(
            textOnly: c => new TextOnly(
                c.Answers.Select(a => TextOnly.Answer.Create(a.Text, a.Id, answersIdToOrder)).ToArray()
            ),
            imageOnly: c => new ImageOnly(
                c.Answers.Select(a => ImageOnly.Answer.Create(a.Image, a.Id, answersIdToOrder)).ToArray()
            ),
            imageAndText: c => new ImageAndText(
                c.Answers.Select(a => ImageAndText.Answer.Create(a.Text, a.Image, a.Id, answersIdToOrder)).ToArray()
            ),
            colorOnly: c => new ColorOnly(
                c.Answers.Select(a => ColorOnly.Answer.Create(a.Color, a.Id, answersIdToOrder)).ToArray()
            ),
            colorAndText: c => new ColorAndText(
                c.Answers.Select(a => ColorAndText.Answer.Create(a.Text, a.Color, a.Id, answersIdToOrder)).ToArray()
            ),
            audioOnly: c => new AudioOnly(
                c.Answers.Select(a => AudioOnly.Answer.Create(a.Audio, a.Id, answersIdToOrder)).ToArray()
            ),
            audioAndText: c => new AudioAndText(
                c.Answers.Select(a => AudioAndText.Answer.Create(a.Text, a.Audio, a.Id, answersIdToOrder)).ToArray()
            )
        );

    public sealed record TextOnly(
        TextOnly.Answer[] Answers
    ) : IVokiTakingQuestionContentPrimitiveDto
    {
        public sealed record Answer(
            string Text,
            string Id,
            ushort OrderInQuestionInSession
        ) : IVokiTakingQuestionAnswerPrimitiveDto
        {
            public static Answer Create(
                GeneralVokiAnswerText text, GeneralVokiAnswerId id,
                IDictionary<GeneralVokiAnswerId, ushort> answersIdToOrder
            ) => new(
                text.ToString(), id.ToString(), answersIdToOrder[id]
            );
        }
    }

    public sealed record ImageOnly(
        ImageOnly.Answer[] Answers
    ) : IVokiTakingQuestionContentPrimitiveDto
    {
        public sealed record Answer(
            string Image,
            string Id,
            ushort OrderInQuestionInSession
        ) : IVokiTakingQuestionAnswerPrimitiveDto
        {
            public static Answer Create(
                GeneralVokiAnswerImageKey image, GeneralVokiAnswerId id,
                IDictionary<GeneralVokiAnswerId, ushort> answersIdToOrder
            ) => new(
                image.ToString(), id.ToString(), answersIdToOrder[id]
            );
        }
    }

    public sealed record ImageAndText(
        ImageAndText.Answer[] Answers
    ) : IVokiTakingQuestionContentPrimitiveDto
    {
        public sealed record Answer(
            string Text,
            string Image,
            string Id,
            ushort OrderInQuestionInSession
        ) : IVokiTakingQuestionAnswerPrimitiveDto
        {
            public static Answer Create(
                GeneralVokiAnswerText text, GeneralVokiAnswerImageKey image, GeneralVokiAnswerId id,
                IDictionary<GeneralVokiAnswerId, ushort> answersIdToOrder
            ) => new(
                text.ToString(), image.ToString(), id.ToString(), answersIdToOrder[id]
            );
        }
    }

    public sealed record ColorOnly(
        ColorOnly.Answer[] Answers
    ) : IVokiTakingQuestionContentPrimitiveDto
    {
        public sealed record Answer(
            string Color,
            string Id,
            ushort OrderInQuestionInSession
        ) : IVokiTakingQuestionAnswerPrimitiveDto
        {
            public static Answer Create(
                HexColor color, GeneralVokiAnswerId id,
                IDictionary<GeneralVokiAnswerId, ushort> answersIdToOrder
            ) => new(
                color.ToString(), id.ToString(), answersIdToOrder[id]
            );
        }
    }

    public sealed record ColorAndText(
        ColorAndText.Answer[] Answers
    ) : IVokiTakingQuestionContentPrimitiveDto
    {
        public sealed record Answer(
            string Text,
            string Color,
            string Id,
            ushort OrderInQuestionInSession
        ) : IVokiTakingQuestionAnswerPrimitiveDto
        {
            public static Answer Create(
                GeneralVokiAnswerText text, HexColor color, GeneralVokiAnswerId id,
                IDictionary<GeneralVokiAnswerId, ushort> answersIdToOrder
            ) => new(
                text.ToString(), color.ToString(), id.ToString(), answersIdToOrder[id]
            );
        }
    }

    public sealed record AudioOnly(
        AudioOnly.Answer[] Answers
    ) : IVokiTakingQuestionContentPrimitiveDto
    {
        public sealed record Answer(
            string Audio,
            string Id,
            ushort OrderInQuestionInSession
        ) : IVokiTakingQuestionAnswerPrimitiveDto
        {
            public static Answer Create(
                GeneralVokiAnswerAudioKey audio, GeneralVokiAnswerId id,
                IDictionary<GeneralVokiAnswerId, ushort> answersIdToOrder
            ) => new(
                audio.ToString(), id.ToString(), answersIdToOrder[id]
            );
        }
    }

    public sealed record AudioAndText(
        AudioAndText.Answer[] Answers
    ) : IVokiTakingQuestionContentPrimitiveDto
    {
        public sealed record Answer(
            string Text,
            string Audio,
            string Id,
            ushort OrderInQuestionInSession
        ) : IVokiTakingQuestionAnswerPrimitiveDto
        {
            public static Answer Create(
                GeneralVokiAnswerText text, GeneralVokiAnswerAudioKey audio, GeneralVokiAnswerId id,
                IDictionary<GeneralVokiAnswerId, ushort> answersIdToOrder
            ) => new(
                text.ToString(), audio.ToString(), id.ToString(), answersIdToOrder[id]
            );
        }
    }
}

public interface IVokiTakingQuestionAnswerPrimitiveDto
{
    public string Id { get; }

    public ushort OrderInQuestionInSession { get; }
}