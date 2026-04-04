using System.ComponentModel;

namespace UserProfilesService.Domain.app_user_aggregate.profile_settings;

public abstract record UserBanner
{
    public abstract UserBannerType Type { get; }

    public record Default : UserBanner
    {
        public override UserBannerType Type => UserBannerType.Default;
    };

    public record FillColor(HexColor Color) : UserBanner
    {
        public override UserBannerType Type => UserBannerType.FillColor;
    };

    public T Match<T>(Func<Default, T> onDefault, Func<FillColor, T> onFillColor) =>
        this switch {
            Default typed => onDefault(typed),
            FillColor typed => onFillColor(typed),
            _ => throw new InvalidEnumArgumentException(Type.ToString(), (int)Type, GetType())
        };
}

public enum UserBannerType
{
    Default,
    FillColor
}