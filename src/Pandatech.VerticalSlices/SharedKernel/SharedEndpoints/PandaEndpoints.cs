using System.Net;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pandatech.VerticalSlices.Context;
using Pandatech.VerticalSlices.SharedKernel.Extensions;
using Pandatech.VerticalSlices.SharedKernel.Helpers;
using PandaVaultClient;

namespace Pandatech.VerticalSlices.SharedKernel.SharedEndpoints;

public static class PandaEndpoints
{
   private const string TagName = "above-board";
   private const string BasePath = $"/{TagName}";

   public static void MapPandaEndpoints(this WebApplication app)
   {
      if (!app.Environment.IsProduction())
      {
         app.MapPandaVaultApi($"{BasePath}/configuration", TagName, ApiHelper.GroupVertical);
      }

      if (app.Environment.IsLocal())
      {
         app.MapDatabaseResetEndpoint();
      }

      app.MapPingEndpoint(true)
         .MapHealthEndpoint(true)
         .MapPrometheusEndpoints(true);
   }

   private static WebApplication MapDatabaseResetEndpoint(this WebApplication app)
   {
      app.MapGet($"{BasePath}/reset-database",
            ([FromServices] DatabaseHelper helper) => helper.ResetDatabase<PostgresContext>())
         .WithTags(TagName)
         .WithGroupName(ApiHelper.GroupVertical);


      return app;
   }

   private static WebApplication MapPrometheusEndpoints(this WebApplication app, bool enabled)
   {
      if (!enabled)
      {
         return app;
      }

      app.MapPrometheusScrapingEndpoint($"{BasePath}/metrics");

      app.UseHealthChecksPrometheusExporter($"{BasePath}/metrics/health",
         options => options.ResultStatusCodes[HealthStatus.Unhealthy] = (int)HttpStatusCode.OK);

      return app;
   }

   private static WebApplication MapPingEndpoint(this WebApplication app, bool enabled)
   {
      if (!enabled)
      {
         return app;
      }

      app.MapGet($"{BasePath}/ping", () => "pong")
         .Produces<string>()
         .WithTags(TagName)
         .WithGroupName(ApiHelper.GroupVertical)
         .WithOpenApi();
      return app;
   }

   private static WebApplication MapHealthEndpoint(this WebApplication app, bool enabled)
   {
      if (!enabled)
      {
         return app;
      }

      app.MapHealthChecks($"{BasePath}/health",
         new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });

      return app;
   }
}
