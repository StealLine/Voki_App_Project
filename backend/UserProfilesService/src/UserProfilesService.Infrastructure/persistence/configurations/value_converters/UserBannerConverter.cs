using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserProfilesService.Application.dtos;
using UserProfilesService.Domain.app_user_aggregate;
using UserProfilesService.Domain.app_user_aggregate.profile_settings;

namespace UserProfilesService.Infrastructure.persistence.configurations.value_converters;

internal sealed class UserBannerConverter : ValueConverter<UserBanner, string>
{
    private static readonly JsonSerializerOptions Options = new() {
        Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
    };

    private static string Serialize(UserBanner banner) =>
        JsonSerializer.Serialize(IUserBannerPrimitiveDto.Create(banner), Options);

    private static UserBanner Deserialize(string json) {
        var dto = JsonSerializer.Deserialize<IUserBannerPrimitiveDto>(json, Options);

        return dto switch {
            FillColorBannerPrimitiveDto fc => new UserBanner.FillColor(new HexColor(fc.Color)),
            _ => new UserBanner.Default()
        };
    }

    public UserBannerConverter() : base(
        banner => Serialize(banner),
        json => Deserialize(json)
    ) { }
}
