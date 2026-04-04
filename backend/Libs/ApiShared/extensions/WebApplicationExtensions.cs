using ApiShared.middlewares;
using Microsoft.AspNetCore.Builder;

namespace ApiShared.extensions;

public static class WebApplicationExtensions
{
    public static void AllowFrontendCors(this WebApplication app) =>
        app.UseCors(ServiceCollectionExtensions.FrontendCorsPolicy);
    
    public static IApplicationBuilder AddExceptionHandlingMiddleware(this IApplicationBuilder app) {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        return app;
    }
}