using ApplicationShared;
using GeneralVokiTakingService.Application.common.repositories;
using GeneralVokiTakingService.Application.common.repositories.taking_sessions;
using GeneralVokiTakingService.Infrastructure.persistence;
using GeneralVokiTakingService.Infrastructure.persistence.repositories;
using GeneralVokiTakingService.Infrastructure.persistence.repositories.taking_sessions;
using InfrastructureShared.Auth;
using InfrastructureShared.Base;
using InfrastructureShared.EfCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GeneralVokiTakingService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment env
    ) {
        return services
            .AddDefaultServices()
            .AddPersistence(configuration, env)
            .AddAuth(configuration)
            .AddMassTransitWithIntegrationEventHandlers(configuration, typeof(Application.DependencyInjection).Assembly)

            .AddIntegrationEventsPublisher();
    }

    private static IServiceCollection AddDefaultServices(this IServiceCollection services) => services
        .AddDateTimeProvider()
        .AddDomainEventsPublisher()
        .AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();

  

    private static IServiceCollection AddIntegrationEventsPublisher(this IServiceCollection services) {
        services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();

        services
            .Scan(scan => scan.FromAssembliesOf(typeof(DependencyInjection))
                .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

        return services;
    }

    private static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment env
    ) {
        string dbConnectionString = configuration.GetConnectionString("GeneralVokiTakingServiceDb")
                                    ?? throw new Exception("Database connection string is not provided.");
        services.AddPgSqlDbContext<GeneralVokiTakingDbContext>(env,dbConnectionString);


        services.AddScoped<IAppUsersRepository, AppUsersRepository>();
        
        services.AddScoped<IGeneralVokiTakenRecordsRepository, GeneralVokiTakenRecordsRepository>();
        
        services.AddScoped<IGeneralVokisRepository, GeneralVokisRepository>();

        services.AddScoped<IBaseTakingSessionsRepository, BaseTakingSessionsRepository>();
        services.AddScoped<ISessionsWithFreeAnsweringRepository, SessionsWithFreeAnsweringRepository>();
        services.AddScoped<ISessionsWithSequentialAnsweringRepository, SessionsWithSequentialAnsweringRepository>();

        return services;
    }
}