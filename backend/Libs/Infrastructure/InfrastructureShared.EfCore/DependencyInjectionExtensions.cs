using ApplicationShared;
using InfrastructureShared.EfCore.unit_of_work;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InfrastructureShared.EfCore;

public static class DependencyInjectionExtensions
{
    public static void AddPgSqlDbContext<T>(
        this IServiceCollection services,
        IWebHostEnvironment env,
        string dbConnectionString
    ) where T : DbContext {
        services.AddDbContext<T>(options => {
                options.UseNpgsql(dbConnectionString);
                
                options.AddInterceptors(new ForUpdateInterceptor());
                
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                
                options.ConfigureDevelopmentExclusive(env);
            }
        );

        services.AddScoped<IUnitOfWorkManager, DbTransactionBasedUnitOfWorkManager<T>>();
    }

    private static void ConfigureDevelopmentExclusive(this DbContextOptionsBuilder options, IWebHostEnvironment env) {
        if (env.IsDevelopment()) {
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
            options.ConfigureWarnings(warning => {
                warning.Log(
                    CoreEventId.FirstWithoutOrderByAndFilterWarning,
                    CoreEventId.RowLimitingOperationWithoutOrderByWarning
                );
            });
        }
    }
}