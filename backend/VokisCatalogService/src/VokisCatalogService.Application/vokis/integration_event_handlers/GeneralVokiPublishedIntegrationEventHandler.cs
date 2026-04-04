using System.Collections.Immutable;
using MassTransit;
using SharedKernel.common.vokis;
using SharedKernel.common.vokis.general_vokis;
using SharedKernel.integration_events.voki_publishing;
using VokimiStorageKeysLib.concrete_keys;
using VokisCatalogService.Application.common.repositories;
using VokisCatalogService.Domain.voki_aggregate;
using VokisCatalogService.Domain.voki_aggregate.type_specific_data;

namespace VokisCatalogService.Application.vokis.integration_event_handlers;

public class GeneralVokiPublishedIntegrationEventHandler : IConsumer<GeneralVokiPublishedIntegrationEvent>
{
    private readonly IVokisRepository _vokisRepository;

    public GeneralVokiPublishedIntegrationEventHandler(IVokisRepository vokisRepository)
    {
        _vokisRepository = vokisRepository;
    }

    public async Task Consume(ConsumeContext<GeneralVokiPublishedIntegrationEvent> context)
    {
        var e = context.Message;
        bool anyAudios = CheckIfVokiHasAnyAudios(e);

        Voki voki = Voki.CreateGeneral(
            e.VokiId,
            new VokiName(e.Name),
            new VokiCoverKey(e.Cover),
            e.PrimaryAuthorId,
            e.CoAuthors.ToImmutableHashSet(),
            VokiManagersIdsSet.Create(e.Managers.ToImmutableHashSet()).AsSuccess(),
            new VokiDetails(e.Description, e.HasMatureContent, e.Language),
            tags: e.Tags.ToImmutableHashSet(),
            e.PublicationDate,
            GeneralVokiInteractionSettings.Create(
                signedInOnlyTaking: e.InteractionSettings.SignedInOnlyTaking,
                resultsVisibility: e.InteractionSettings.ResultsVisibility,
                showResultsDistribution: e.InteractionSettings.ShowResultsDistribution
            ).AsSuccess(),
            new GeneralVokiTypeSpecificData(
                questionsCount: (ushort)e.Questions.Length,
                resultsCount: (ushort)e.Results.Length,
                anyAudios: anyAudios,
                forceSequentialAnswering: e.ForceSequentialAnswering,
                shuffleQuestions: e.ShuffleQuestions
            )
        );
        await _vokisRepository.Add(voki, context.CancellationToken);
    }

    private static bool CheckIfVokiHasAnyAudios(GeneralVokiPublishedIntegrationEvent v)=>
        v.Questions.Any(q => q.HasAnyAudio);
}
