using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using System.Configuration;

namespace GenericApi.Extensions;

public static class HealthCheckExtensions
{
    public static IEndpointRouteBuilder MapCustomHealthChecks(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
        {
            Predicate = r => r.Name.Contains("self")
        });

        return endpoints;
    }

    public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services
                .AddHealthChecks()
                .AddSqlServer(
                        connectionString: configuration["ConnectionStrings:DefaultConnection"],
                        healthQuery: "SELECT 1;",
                        name: "sql",
                        failureStatus: HealthStatus.Degraded,
                        tags: new string[] { "db", "sql", "sql server" });
        return services;
    }
}
