using System.Text.Json.Serialization;
using SharedKernel.common;
using SharedKernel.common.vokis;

namespace SharedKernel.integration_events.voki_publishing;

// @formatter:off
[JsonDerivedType(typeof(GeneralVokiPublishedIntegrationEvent), typeDiscriminator: nameof(GeneralVokiPublishedIntegrationEvent))]
// @formatter:on
public abstract record class BaseVokiPublishedIntegrationEvent(
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
    DateTime PublicationDate
) : IIntegrationEvent;