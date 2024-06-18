using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Pandatech.VerticalSlices.SharedKernel.Extensions;

public static class OpenTelemetryExtension
{
   public static WebApplicationBuilder ConfigureOpenTelemetry(this WebApplicationBuilder builder)
   {
         builder.Logging.AddOpenTelemetry(x =>
         {
            x.IncludeScopes = true;
            x.IncludeFormattedMessage = true;
         });

         builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(builder.Environment.ApplicationName))
            .WithMetrics(metrics =>
            {
               metrics.AddRuntimeInstrumentation()
                  .AddAspNetCoreInstrumentation()
                  .AddHttpClientInstrumentation()
                  .AddPrometheusExporter();
            })
            .WithTracing(tracing =>
            {
               tracing.AddAspNetCoreInstrumentation()
                  .AddHttpClientInstrumentation()
                  .AddGrpcClientInstrumentation();
            });

         return builder;
      }
}