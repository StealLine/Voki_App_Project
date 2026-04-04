using ApplicationShared;
using InfrastructureShared.Auth;
using InfrastructureShared.Base;
using InfrastructureShared.Base.domain_events_publisher;
using InfrastructureShared.EfCore;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VokiRatingsService.Application.background_services;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Infrastructure.persistence;
using VokiRatingsService.Infrastructure.persistence.repositories;

namespace VokiRatingsService.Infrastructure;

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
            .AddIntegrationEventsPublisher()
            .AddBackgroundServices();
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
        string dbConnectionString = configuration.GetConnectionString("VokiRatingsServiceDb")
                                    ?? throw new Exception("Database connection string is not provided.");
        services.AddPgSqlDbContext<VokiRatingsDbContext>(env, dbConnectionString);

        services.AddScoped<IAppUsersRepository, AppUsersRepository>();
        services.AddScoped<IVokisRepository, VokisRepository>();
        services.AddScoped<IRatingsRepository, RatingsRepository>();
        services.AddScoped<IVokiRatingsSnapshotRepository, VokiRatingsSnapshotRepository>();
        services.AddScoped<IUpdateRatingsSnapshotMarkerRepository, UpdateRatingsSnapshotMarkerRepository>();


        return services;
    }

    private static IServiceCollection AddBackgroundServices(this IServiceCollection services) {
        services.AddHostedService<RatingsSnapshotUpdaterBackgroundService>();
        return services;
    }
}