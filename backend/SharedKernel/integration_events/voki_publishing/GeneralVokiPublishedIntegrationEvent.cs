using System.Text.Json.Serialization;
using SharedKernel.common;
using SharedKernel.common.vokis.general_vokis;

namespace SharedKernel.integration_events.voki_publishing;

public record class GeneralVokiPublishedIntegrationEvent(
    VokiId VokiId,
    AppUserId PrimaryAuthorId,
    AppUserId[] CoAuthors,
    AppUserId[] Managers,
    string Name,
    string Cover,
    string Description,
    bool HasMatureContent,
    Language Language,
    VokiTagId[] Tags,
    DateTime InitializingDate,
    DateTime PublicationDate,
    //Voki Type specific 
    GeneralVokiQuestionIntegrationEventDto[] Questions,
    bool ForceSequentialAnswering,
    bool ShuffleQuestions,
    GeneralVokiResultIntegrationEventDto[] Results,
    GeneralVokiInteractionSettingsIntegrationEventDto InteractionSettings
) : BaseVokiPublishedIntegrationEvent(
    VokiId, PrimaryAuthorId, CoAuthors, Managers,
    Name, Cover, Description,
    HasMatureContent, Language, Tags,
    InitializingDate, PublicationDate
);

public record GeneralVokiInteractionSettingsIntegrationEventDto(
    bool SignedInOnlyTaking,
    GeneralVokiResultsVisibility ResultsVisibility,
    bool ShowResultsDistribution
);

public sealed record GeneralVokiResultIntegrationEventDto(
    GeneralVokiResultId Id,
    string Name,
    string Text,
    string? DraftVokiImageKey
);

public sealed record GeneralVokiQuestionIntegrationEventDto(
    GeneralVokiQuestionId Id,
    string Text,
    string[] Images,
    Double ImagesAspectRatio,
    ushort OrderInVoki,
    bool ShuffleAnswers,
    ushort MinAnswersCount,
    ushort MaxAnswersCount,
    IQuestionContentIntegrationEventDto Content,
    bool HasAnyAudio
);
// @formatter:off
[JsonDerivedType(typeof(TextOnlyQuestionIntegrationEventDto), typeDiscriminator: nameof(TextOnlyQuestionIntegrationEventDto))]
[JsonDerivedType(typeof(ImageOnlyQuestionIntegrationEventDto), typeDiscriminator: nameof(ImageOnlyQuestionIntegrationEventDto))]
[JsonDerivedType(typeof(ImageAndTextQuestionIntegrationEventDto),typeDiscriminator: nameof(ImageAndTextQuestionIntegrationEventDto))]
[JsonDerivedType(typeof(ColorOnlyQuestionIntegrationEventDto), typeDiscriminator: nameof(ColorOnlyQuestionIntegrationEventDto))]
[JsonDerivedType(typeof(ColorAndTextQuestionIntegrationEventDto),typeDiscriminator: nameof(ColorAndTextQuestionIntegrationEventDto))]
[JsonDerivedType(typeof(AudioOnlyQuestionIntegrationEventDto), typeDiscriminator: nameof(AudioOnlyQuestionIntegrationEventDto))]
[JsonDerivedType(typeof(AudioAndTextQuestionIntegrationEventDto),typeDiscriminator: nameof(AudioAndTextQuestionIntegrationEventDto))]
// @formatter:on
public interface IQuestionContentIntegrationEventDto;

public interface IQuestionAnswerIntegrationEventDto
{
    public GeneralVokiAnswerId Id { get; }
    public ushort Order { get; }
    public GeneralVokiResultId[] RelatedResultIds { get; }
}

public sealed record TextOnlyQuestionIntegrationEventDto(
    TextOnlyQuestionIntegrationEventDto.Answer[] Answers
) : IQuestionContentIntegrationEventDto
{
    public record Answer(
        GeneralVokiAnswerId Id,
        ushort Order,
        GeneralVokiResultId[] RelatedResultIds,
        string Text
    ) : IQuestionAnswerIntegrationEventDto;
}

public sealed record ImageOnlyQuestionIntegrationEventDto(
    ImageOnlyQuestionIntegrationEventDto.Answer[] Answers
) : IQuestionContentIntegrationEventDto
{
    public record Answer(
        GeneralVokiAnswerId Id,
        ushort Order,
        GeneralVokiResultId[] RelatedResultIds,
        string Image
    ) : IQuestionAnswerIntegrationEventDto;
}

public sealed record ImageAndTextQuestionIntegrationEventDto(
    ImageAndTextQuestionIntegrationEventDto.Answer[] Answers
) : IQuestionContentIntegrationEventDto
{
    public record Answer(
        GeneralVokiAnswerId Id,
        ushort Order,
        GeneralVokiResultId[] RelatedResultIds,
        string Text,
        string Image
    ) : IQuestionAnswerIntegrationEventDto;
}

public sealed record ColorOnlyQuestionIntegrationEventDto(
    ColorOnlyQuestionIntegrationEventDto.Answer[] Answers
) : IQuestionContentIntegrationEventDto
{
    public record Answer(
        GeneralVokiAnswerId Id,
        ushort Order,
        GeneralVokiResultId[] RelatedResultIds,
        string Color
    ) : IQuestionAnswerIntegrationEventDto;
}

public sealed record ColorAndTextQuestionIntegrationEventDto(
    ColorAndTextQuestionIntegrationEventDto.Answer[] Answers
) : IQuestionContentIntegrationEventDto
{
    public record Answer(
        GeneralVokiAnswerId Id,
        ushort Order,
        GeneralVokiResultId[] RelatedResultIds,
        string Text,
        string Color
    ) : IQuestionAnswerIntegrationEventDto;
}

public sealed record AudioOnlyQuestionIntegrationEventDto(
    AudioOnlyQuestionIntegrationEventDto.Answer[] Answers
) : IQuestionContentIntegrationEventDto
{
    public record Answer(
        GeneralVokiAnswerId Id,
        ushort Order,
        GeneralVokiResultId[] RelatedResultIds,
        string Audio
    ) : IQuestionAnswerIntegrationEventDto;
}

public sealed record AudioAndTextQuestionIntegrationEventDto(
    AudioAndTextQuestionIntegrationEventDto.Answer[] Answers
) : IQuestionContentIntegrationEventDto
{
    public record Answer(
        GeneralVokiAnswerId Id,
        ushort Order,
        GeneralVokiResultId[] RelatedResultIds,
        string Text,
        string Audio
    ) : IQuestionAnswerIntegrationEventDto;
}