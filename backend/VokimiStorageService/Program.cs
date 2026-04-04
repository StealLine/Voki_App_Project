using System.Reflection;
using ApiShared.extensions;
using InfrastructureShared.Auth;
using InfrastructureShared.Base;
using VokimiStorageService.extensions;

namespace VokimiStorageService;

public class Program
{
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseDefaultServiceProvider((_, options) => {
            options.ValidateScopes = false;
            options.ValidateOnBuild = true;
        });

        builder.AddConfiguredLogging();
        builder.Services
            .AddAuth(builder.Configuration)
            .AddS3Storage(builder.Configuration)
            .AddPresentation(builder.Configuration)
            .AddEndpoints(Assembly.GetExecutingAssembly());

        var app = builder.Build();

        if (app.Environment.IsDevelopment()) {
            app.MapOpenApi();
        }
        else {
            app.UseHttpsRedirection();
        }

        app.AddExceptionHandlingMiddleware();
        app.MapEndpointGroups();

        app.Run();
    }
}