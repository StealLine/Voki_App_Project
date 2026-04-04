using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.content_types;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;
using SharedKernel.common.vokis.general_vokis;
using SharedKernel.integration_events.voki_publishing;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;
using VokimiStorageKeysLib.concrete_keys;
using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.events;

public record GeneralVokiPublishedEvent(
    VokiId VokiId,
    AppUserId PrimaryAuthorId,
    VokiCoAuthorIdsSet CoAuthors,
    ImmutableHashSet<AppUserId> UserIdsToBecomeManagers,
    VokiName Name,
    VokiCoverKey Cover,
    VokiDetails Details,
    VokiTagsSet Tags,
    DateTime InitializingDate,
    DateTime PublicationDate,
    //Voki Type specific 
    VokiTakingProcessSettings TakingProcessSettings,
    GeneralVokiInteractionSettings InteractionSettings,
    QuestionDomainEventDto[] Questions,
    ResultDomainEventDto[] Results
) : IDomainEvent;

public record QuestionDomainEventDto(
    GeneralVokiQuestionId Id,
    VokiQuestionText Text,
    VokiQuestionImagesSet ImageSet,
    VokiQuestionOrder OrderInVoki,
    bool ShuffleAnswers,
    QuestionAnswersCountLimit AnswersCountLimit,
    IQuestionContentIntegrationEventDto Content,
    bool HasAnyAudio
);

public record ResultDomainEventDto(
    GeneralVokiResultId Id,
    VokiResultName Name,
    VokiResultText Text,
    GeneralVokiResultImageKey? Image
);