using System.Net;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using PandaWebApi.Contexts;
using PandaWebApi.Helpers;

namespace PandaWebApi.Extensions;

public static class EndpointExtensions
{
    public static void MapPingApi(this WebApplication app)
    {
        app.MapGet("/ping", () => "pong").WithTags("Above Board");
    }

    public static void MapDatabaseResetApi(this WebApplication app)
    {
        if (app.Environment.IsEnvironment("Local"))
        {
            app.MapGet("/reset-database", (DatabaseHelper helper) => helper.ResetDatabase<PostgresContext>())
                .WithTags("Above Board");
        }
    }

    public static void MapHealthApi(this WebApplication app)
    {
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        }).WithTags("Above Board");
    }
    
    public static void MapErrorApi(this WebApplication app)
    {
        app.MapGet("/error", new Func<object>(() => throw new HttpRequestException("Not found",null, HttpStatusCode.NotFound)
        )).WithTags("Above Board");
    }
}