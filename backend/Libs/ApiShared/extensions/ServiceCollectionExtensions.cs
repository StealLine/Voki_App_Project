using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiShared.extensions;

public static class ServiceCollectionExtensions
{
    public const string FrontendCorsPolicy = "FrontendCorsPolicy";

    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration) {
        services.AddFrontendCors(configuration);
        services.AddHttpContextAccessor();
        services.AddOpenApi();
        services.ConfigureHttpJsonOptions(options => {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        return services;
    }

    private static IServiceCollection AddFrontendCors(this IServiceCollection services, IConfiguration configuration) {
        string frontendUrl = configuration["FrontendUrl"] ?? throw new Exception("FrontendUrl is not configured");

        services.AddCors(options => {
            options.AddPolicy(FrontendCorsPolicy, policy => {
                policy
                    .WithOrigins(frontendUrl)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }
}