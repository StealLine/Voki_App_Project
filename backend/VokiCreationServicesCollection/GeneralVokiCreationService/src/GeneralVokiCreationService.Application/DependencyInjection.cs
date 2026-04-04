using Microsoft.Extensions.DependencyInjection;
using VokiCreationServicesLib.Application;

namespace GeneralVokiCreationService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services) {
        services.AddLibMessaging();
        services.AddApplicationMessaging(typeof(DependencyInjection));
        services.AddApplicationDomainEventHandlers(typeof(DependencyInjection));
        services.AddApplicationStepHandlers();
        return services;
    }
}