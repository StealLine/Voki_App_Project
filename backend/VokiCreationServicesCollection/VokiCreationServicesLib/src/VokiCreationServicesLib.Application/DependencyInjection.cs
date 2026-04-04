using Microsoft.Extensions.DependencyInjection;

namespace VokiCreationServicesLib.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddLibMessaging(this IServiceCollection services) {
        services.AddApplicationMessaging(typeof(DependencyInjection));
        services.AddApplicationDomainEventHandlers(typeof(DependencyInjection));

        return services;
    }
}