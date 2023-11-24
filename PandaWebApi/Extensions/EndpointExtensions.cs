using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using PandaWebApi.Contexts;
using PandaWebApi.Helpers;

namespace PandaWebApi.Extensions;

public static class EndpointExtensions
{
    public static WebApplication AddPingEndpoint(this WebApplication app)
    {
        app.MapGet("/ping", () => "pong").WithTags("Above Board");
        return app;
    }

    public static WebApplication AddDatabaseResetEndpoints(this WebApplication app)
    {
        if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Local"))
        {
            app.MapGet("/reset-database", (DatabaseHelper helper) => helper.ResetDatabase<PostgresContext>())
                .WithTags("Above Board");
        }
        return app;
    }

    public static WebApplication AddHealthCheckEndpoint(this WebApplication app)
    {
        app.MapHealthChecks("/health", new HealthCheckOptions()
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        }).WithTags("Above Board");
        return app;
    }
}
