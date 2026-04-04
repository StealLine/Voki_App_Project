using System.Text.Json.Serialization;
using UserProfilesService.Domain.app_user_aggregate.profile_settings;

namespace UserProfilesService.Application.dtos;

[JsonDerivedType(typeof(DefaultBannerPrimitiveDto), typeDiscriminator: nameof(UserBannerType.Default))]
[JsonDerivedType(typeof(FillColorBannerPrimitiveDto), typeDiscriminator: nameof(UserBannerType.FillColor))]
public interface IUserBannerPrimitiveDto
{
    public sealed static IUserBannerPrimitiveDto Create(UserBanner b) => b.Match<IUserBannerPrimitiveDto>(
        onDefault: (_) => new DefaultBannerPrimitiveDto(),
        onFillColor: (typed) => new FillColorBannerPrimitiveDto(typed.Color.ToString())
    );
}

public record DefaultBannerPrimitiveDto : IUserBannerPrimitiveDto;

public record FillColorBannerPrimitiveDto(string Color) : IUserBannerPrimitiveDto;