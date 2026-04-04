using AuthService.Infrastructure;

namespace AuthService.Api;

internal static class DependencyInjection
{
    internal static IServiceCollection AddFrontendConfig(this IServiceCollection services, IConfiguration configuration) {
        var frontendConfig = configuration.GetSection("FrontendConfig").Get<FrontendConfig>();
        if (frontendConfig is null) {
            throw new Exception("FrontendConfig is not configured");
        }

        return services;
    }
}