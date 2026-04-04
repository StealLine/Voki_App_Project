using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.events;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.content_types;
using SharedKernel.integration_events.voki_publishing;

namespace GeneralVokiCreationService.Application.domain_to_integration_event_mappers;

public static class VokiPublishedEventMapper
{
    public static GeneralVokiResultIntegrationEventDto[] ResultIntegrationEventDtoArray(
        ResultDomainEventDto[] results
    ) => results
        .Select(r => new GeneralVokiResultIntegrationEventDto(
            r.Id,
            r.Name.ToString(),
            r.Text.ToString(),
            r.Image?.ToString() ?? null
        ))
        .ToArray();

    public static GeneralVokiQuestionIntegrationEventDto[] QuestionIntegrationEventDtoArray(
        QuestionDomainEventDto[] questions
    ) => questions
        .Select(QuestionDtoFromQuestion)
        .ToArray();


    private static GeneralVokiQuestionIntegrationEventDto QuestionDtoFromQuestion(QuestionDomainEventDto q) => new(
        q.Id,
        q.Text.ToString(),
        Images: q.ImageSet.Keys.Select(i => i.ToString()).ToArray(),
        ImagesAspectRatio: q.ImageSet.AspectRatio.GetRatio(),
        q.OrderInVoki.Value,
        ShuffleAnswers: q.ShuffleAnswers,
        MinAnswersCount: q.AnswersCountLimit.MinAnswers,
        MaxAnswersCount: q.AnswersCountLimit.MaxAnswers,
        Content: q.Content,
        HasAnyAudio: q.HasAnyAudio
    );
}