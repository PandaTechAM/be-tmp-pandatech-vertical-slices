using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using PandaVaultClient;
using PandaWebApi.Contexts;
using PandaWebApi.Helpers;

namespace PandaWebApi.Extensions;

public static class EndpointExtensions
{
    public static void MapPandaStandardEndpoints(this WebApplication app)
    {
        app.MapHealthApi()
            .MapPingApi()
            .MapPandaVaultApi(); //optional

        if (app.Environment.IsLocal())
        {
            app.MapDatabaseResetApi();
        }
    }

    private static WebApplication MapPingApi(this WebApplication app)
    {
        app.MapGet("/above-board/ping", () => "pong").WithTags("Above Board");
        return app;
    }

    private static WebApplication MapDatabaseResetApi(this WebApplication app)
    {
        app.MapGet("/above-board/reset-database", ([FromServices] DatabaseHelper helper) => helper.ResetDatabase<PostgresContext>())
            .WithTags("Above Board");


        return app;
    }

    private static WebApplication MapHealthApi(this WebApplication app)
    {
        app.MapHealthChecks("/above-board/panda-wellness", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        }).WithTags("Above Board");
        return app;
    }
}