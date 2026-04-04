using System.Runtime.CompilerServices;
using GeneralVokiTakingService.Application.common.repositories;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.general_voki_aggregate.questions;
using GeneralVokiTakingService.Domain.general_voki_aggregate.questions.answers;
using GeneralVokiTakingService.Domain.general_voki_aggregate.questions.content;
using MassTransit;
using SharedKernel.common.vokis;
using SharedKernel.common.vokis.general_vokis;
using SharedKernel.integration_events.voki_publishing;
using VokimiStorageKeysLib.concrete_keys;
using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiTakingService.Application.general_vokis.integration_event_handlers;

public class GeneralVokiPublishedIntegrationEventHandler : IConsumer<GeneralVokiPublishedIntegrationEvent>
{
    private readonly IGeneralVokisRepository _generalVokisRepository;

    public GeneralVokiPublishedIntegrationEventHandler(IGeneralVokisRepository generalVokisRepository) {
        _generalVokisRepository = generalVokisRepository;
    }

    public async Task Consume(ConsumeContext<GeneralVokiPublishedIntegrationEvent> context) {
        var e = context.Message;
        GeneralVoki voki = GeneralVoki.CreateNew(
            e.VokiId,
            new VokiCoverKey(e.Cover),
            name: new VokiName(e.Name),
            e.Questions.Select(QuestionFromEventDto).ToImmutableArray(),
            e.Results.Select(ResultFromEventDto).ToImmutableArray(),
            forceSequentialAnswering: e.ForceSequentialAnswering,
            shuffleQuestions: e.ShuffleQuestions,
            GeneralVokiInteractionSettings.Create(
                signedInOnlyTaking: e.InteractionSettings.SignedInOnlyTaking,
                resultsVisibility: e.InteractionSettings.ResultsVisibility,
                showResultsDistribution: e.InteractionSettings.ShowResultsDistribution
            ).AsSuccess(),
            VokiManagersIdsSet.Create(e.Managers.ToImmutableHashSet()).AsSuccess()
        );
        await _generalVokisRepository.Add(voki, context.CancellationToken);
    }

    private static VokiResult ResultFromEventDto(GeneralVokiResultIntegrationEventDto r) => new(
        r.Id, r.Name, r.Text,
        r.DraftVokiImageKey is null ? null : new GeneralVokiResultImageKey(r.DraftVokiImageKey)
    );

    private static VokiQuestion QuestionFromEventDto(GeneralVokiQuestionIntegrationEventDto q) => new(
        q.Id, q.Text,
        ImageSetFromEventDto(q.Images, q.ImagesAspectRatio),
        VokiQuestionOrder.Create(q.OrderInVoki).AsSuccess(), q.ShuffleAnswers,
        new QuestionAnswersCountLimit(minAnswers: q.MinAnswersCount, maxAnswers: q.MaxAnswersCount),
        QuestionContentFromEventDto(q.Content)
    );

    private static VokiQuestionImagesSet ImageSetFromEventDto(string[] images, double aspectRatio) =>
        new(images.Select(key => new GeneralVokiQuestionImageKey(key)).ToImmutableArray(), aspectRatio);

    private static GeneralVokiQuestionContent QuestionContentFromEventDto(
        IQuestionContentIntegrationEventDto content
    ) => content switch {
        TextOnlyQuestionIntegrationEventDto t => new GeneralVokiQuestionContent.TextOnly(
            t.Answers
                .Select(a => new BaseQuestionAnswer.TextOnly(
                    GeneralVokiAnswerText.Create(a.Text).AsSuccess(),
                    a.Id,
                    a.Order,
                    a.RelatedResultIds.ToImmutableHashSet()
                ))
                .ToImmutableArray()),
        ImageOnlyQuestionIntegrationEventDto t => new GeneralVokiQuestionContent.ImageOnly(
            t.Answers
                .Select(a => new BaseQuestionAnswer.ImageOnly(
                    new GeneralVokiAnswerImageKey(a.Image),
                    a.Id,
                    a.Order,
                    a.RelatedResultIds.ToImmutableHashSet()
                ))
                .ToImmutableArray()
        ),
        ImageAndTextQuestionIntegrationEventDto t => new GeneralVokiQuestionContent.ImageAndText(
            t.Answers
                .Select(a => new BaseQuestionAnswer.ImageAndText(
                    new GeneralVokiAnswerImageKey(a.Image),
                    GeneralVokiAnswerText.Create(a.Text).AsSuccess(),
                    a.Id,
                    a.Order,
                    a.RelatedResultIds.ToImmutableHashSet()
                ))
                .ToImmutableArray()
        ),
        ColorOnlyQuestionIntegrationEventDto t => new GeneralVokiQuestionContent.ColorOnly(
            t.Answers
                .Select(a => new BaseQuestionAnswer.ColorOnly(
                    new HexColor(a.Color),
                    a.Id,
                    a.Order,
                    a.RelatedResultIds.ToImmutableHashSet()
                ))
                .ToImmutableArray()
        ),
        ColorAndTextQuestionIntegrationEventDto t => new GeneralVokiQuestionContent.ColorAndText(
            t.Answers
                .Select(a => new BaseQuestionAnswer.ColorAndText(
                    new HexColor(a.Color),
                    GeneralVokiAnswerText.Create(a.Text).AsSuccess(),
                    a.Id,
                    a.Order,
                    a.RelatedResultIds.ToImmutableHashSet()
                ))
                .ToImmutableArray()
        ),
        AudioOnlyQuestionIntegrationEventDto t => new GeneralVokiQuestionContent.AudioOnly(
            t.Answers
                .Select(a => new BaseQuestionAnswer.AudioOnly(
                    new GeneralVokiAnswerAudioKey(a.Audio),
                    a.Id,
                    a.Order,
                    a.RelatedResultIds.ToImmutableHashSet()
                ))
                .ToImmutableArray()
        ),
        AudioAndTextQuestionIntegrationEventDto t => new GeneralVokiQuestionContent.AudioAndText(
            t.Answers
                .Select(a => new BaseQuestionAnswer.AudioAndText(
                    new GeneralVokiAnswerAudioKey(a.Audio),
                    GeneralVokiAnswerText.Create(a.Text).AsSuccess(),
                    a.Id,
                    a.Order,
                    a.RelatedResultIds.ToImmutableHashSet()
                ))
                .ToImmutableArray()
        ),
        _ => throw new SwitchExpressionException(content)
    };
}