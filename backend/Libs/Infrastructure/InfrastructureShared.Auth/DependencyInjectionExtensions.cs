using ApplicationShared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfrastructureShared.Auth;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration) {
        var jwtTokenConfig = configuration.GetSection("JwtTokenConfig").Get<JwtTokenConfig>();
        if (jwtTokenConfig is null) {
            throw new Exception("JWT token config not configured");
        }

        services.AddSingleton(jwtTokenConfig);
        services.AddScoped<ITokenParser, TokenParser>();
        services.AddScoped<IUserCtxProvider, UserCtxProvider>();
        return services;
    }
}