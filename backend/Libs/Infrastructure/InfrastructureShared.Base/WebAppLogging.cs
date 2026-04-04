using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace InfrastructureShared.Base;

public static class WebAppLogging
{
    public static void AddConfiguredLogging(this  WebApplicationBuilder builder) {
        builder.Logging.AddFilter(
            "Microsoft.EntityFrameworkCore.Database.Transaction",
            LogLevel.Debug
        );
        
        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService(DependencyInjectionExtensions.GetServiceName(builder.Configuration))
                .AddAttributes([
                    new("service.entry_assembly_name", Assembly.GetEntryAssembly()!.GetName().Name!)
                ])
            )
            // .WithLogging(l=>l.AddConsoleExporter())
            .WithTracing(tracing =>
                    tracing
                        .AddAspNetCoreInstrumentation()
                // .AddNpgsql()
                // .AddConsoleExporter()
            );
    }
}