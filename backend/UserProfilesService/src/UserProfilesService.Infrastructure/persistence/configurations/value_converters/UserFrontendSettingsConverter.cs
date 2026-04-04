using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserProfilesService.Domain.app_user_aggregate;

namespace UserProfilesService.Infrastructure.persistence.configurations.value_converters;

internal sealed class UserFrontendSettingsConverter : ValueConverter<UserFrontendSettings, string>
{
    private static readonly JsonSerializerOptions Options = new() {
        Converters = { new JsonStringEnumConverter() }
    };

    public UserFrontendSettingsConverter() : base(
        settings => JsonSerializer.Serialize(settings, Options),
        json => JsonSerializer.Deserialize<UserFrontendSettings>(json, Options)!
    ) { }
}