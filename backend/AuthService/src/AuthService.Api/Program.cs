using System.Reflection;
using AuthService.Application;
using AuthService.Infrastructure;
using InfrastructureShared.Base;

namespace AuthService.Api;

public class Program
{
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseDefaultServiceProvider((_, options) => {
            options.ValidateScopes = false;
            options.ValidateOnBuild = true;
        });

        builder.AddConfiguredLogging();
        builder.Services.AddFrontendConfig(builder.Configuration);

        builder.Services
            .AddApplication()
            .AddInfrastructure(builder.Configuration, builder.Environment)
            .AddPresentation(builder.Configuration)
            .AddEndpoints(Assembly.GetExecutingAssembly())
            ;

        var app = builder.Build();
        if (app.Environment.IsDevelopment()) {
            app.MapOpenApi();
        }
        else {
            app.UseHttpsRedirection();
        }

        app.AddExceptionHandlingMiddleware();
        app.MapEndpointGroups();
        app.AllowFrontendCors();
        
        app.Run();
    }
}