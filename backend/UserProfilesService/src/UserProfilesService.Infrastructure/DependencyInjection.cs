using Amazon.Runtime;
using Amazon.S3;
using ApplicationShared;
using InfrastructureShared.Auth;
using InfrastructureShared.Base;
using InfrastructureShared.EfCore;
using InfrastructureShared.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserProfilesService.Application.common;
using UserProfilesService.Application.common.repositories;
using UserProfilesService.Infrastructure.persistence;
using UserProfilesService.Infrastructure.persistence.repositories;
using UserProfilesService.Infrastructure.storage;

namespace UserProfilesService.Infrastructure;

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
            .AddS3(configuration);
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
        string dbConnectionString = configuration.GetConnectionString("UserProfilesServiceDb")
                                    ?? throw new Exception("Database connection string is not provided.");
        
        services.AddPgSqlDbContext<UserProfilesDbContext>(env, dbConnectionString);
        services.AddScoped<IAppUsersRepository, AppUsersRepository>();

        return services;
    }

    private static IServiceCollection AddS3(this IServiceCollection services, IConfiguration configuration) {
        var s3Config = configuration.GetSection("S3").Get<S3Config>();
        if (s3Config is null) {
            throw new Exception("S3 is not configured");
        }

        services.AddSingleton<IAmazonS3>(_ => new AmazonS3Client(
            new BasicAWSCredentials(s3Config.AccessKey, s3Config.SecretKey),
            new AmazonS3Config { ServiceURL = s3Config.ServiceUrl }
        ));

        services.AddSingleton(s3Config.MainBucket); //S3MainBucketConf
        services.AddScoped<IMainStorageBucket, MainStorageBucket>();

        return services;
    }
}