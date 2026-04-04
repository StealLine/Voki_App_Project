using ApplicationShared;
using AuthService.Application.abstractions;
using AuthService.Application.background_services;
using AuthService.Application.common.repositories;
using AuthService.Infrastructure.auth;
using AuthService.Infrastructure.email_service;
using AuthService.Infrastructure.persistence;
using AuthService.Infrastructure.persistence.repositories;
using InfrastructureShared.Auth;
using InfrastructureShared.Base;
using InfrastructureShared.EfCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Infrastructure;

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
            .AddPasswordHasherAndTokenGenerator(configuration)
            .AddEmailService(configuration)
            .AddFrontendConfig(configuration)
            .AddMassTransitWithIntegrationEventHandlers(configuration, typeof(Application.DependencyInjection).Assembly)
            .AddIntegrationEventsPublisher()
            .AddBackgroundServices();
    }

    private static IServiceCollection AddDefaultServices(this IServiceCollection services) => services
        .AddDateTimeProvider()
        .AddDomainEventsPublisher()
        .AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();

    private static IServiceCollection AddPasswordHasherAndTokenGenerator(
        this IServiceCollection services,
        IConfiguration configuration
    ) {
        AuthPrivateKeyConfig? privateKeyConf = configuration.GetSection("AuthPrivateKey").Get<AuthPrivateKeyConfig>();
        if (privateKeyConf is null) {
            throw new Exception("Email service is not configured");
        }

        return services
            .AddSingleton(privateKeyConf)
            .AddScoped<IPasswordHasher, PasswordHasher>()
            .AddScoped<ITokenGenerator, TokenGenerator>();
    }


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
        string dbConnectionString = configuration.GetConnectionString("AuthServiceDb")
                                    ?? throw new Exception("Database connection string is not provided.");
        services.AddPgSqlDbContext<AuthDbContext>(env, dbConnectionString);
        
        services.AddScoped<IAppUsersRepository, AppUsersRepository>();
        services.AddScoped<IUnconfirmedUsersRepository, UnconfirmedUsersRepository>();

        return services;
    }

    private static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration) {
        var emailServiceConfig = configuration.GetSection("EmailServiceConfig").Get<EmailServiceConfig>();
        if (emailServiceConfig is null) {
            throw new Exception("Email service is not configured");
        }

        services.AddSingleton(emailServiceConfig);
        services.AddTransient<IEmailService, EmailService>();

        return services;
    }

    private static IServiceCollection AddFrontendConfig(
        this IServiceCollection services, IConfiguration configuration
    ) {
        var frontendConfig = configuration.GetSection("FrontendConfig").Get<FrontendConfig>();
        if (frontendConfig is null) {
            throw new Exception("No frontend config provided");
        }

        services.AddSingleton(frontendConfig);
        return services;
    }

    private static IServiceCollection AddBackgroundServices(this IServiceCollection services) {
        services.AddHostedService<UnconfirmedUsersCleanupBackgroundService>();
        return services;
    }
}