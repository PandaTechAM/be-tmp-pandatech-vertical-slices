using System.Net;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using PandaVaultClient;
using PandaWebApi.Contexts;
using PandaWebApi.Helpers;

namespace PandaWebApi.Extensions;

public static class EndpointExtensions
{
    public static void MapPandaStandardEndpoints(this WebApplication app)
    {
        app.MapHealthApi()
            .MapDatabaseResetApi()
            .MapPingApi()
            .MapPandaVaultApi(); //optional
    }

    private static WebApplication MapPingApi(this WebApplication app)
    {
        app.MapGet("/ping", () => "pong").WithTags("Above Board");
        return app;
    }

    private static WebApplication MapDatabaseResetApi(this WebApplication app)
    {
        if (app.Environment.IsEnvironment("Local"))
        {
            app.MapGet("/reset-database", (DatabaseHelper helper) => helper.ResetDatabase<PostgresContext>())
                .WithTags("Above Board");
        }

        return app;
    }

    private static WebApplication MapHealthApi(this WebApplication app)
    {
        app.MapHealthChecks("/panda-wellness", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        }).WithTags("Above Board");
        return app;
    }
}