using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

namespace Pandatech.VerticalSlices.SharedKernel.Extensions;

public static class MassTransitExtension
{
   public static WebApplicationBuilder AddMassTransit(this WebApplicationBuilder builder, params Assembly[] assemblies)
   {
      builder.Services.AddMassTransit(x =>
      {
         x.AddConsumers(assemblies);
         x.SetKebabCaseEndpointNameFormatter();
         x.UsingRabbitMq((context, cfg) =>
         {
            cfg.Host(builder.Configuration.GetRabbitMqUrl());
            cfg.ConfigureEndpoints(context);
            cfg.UseMessageRetry(r =>
               r.Exponential(5, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(2)));
         });
      });

      builder
         .Services
         .AddHealthChecks()
         .AddCheck<RabbitMqHealthCheck>("rabbit_mq", timeout: TimeSpan.FromSeconds(3));

      return builder;
   }
}

public class RabbitMqHealthCheck(IConfiguration configuration) : IHealthCheck
{
   public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
      CancellationToken cancellationToken = new())
   {
      var rmqConnectionString = configuration.GetRabbitMqUrl();
      var factory = new ConnectionFactory
      {
         Uri = new Uri(rmqConnectionString),
         AutomaticRecoveryEnabled = true
      };
      var connection = default(IConnection);
      try
      {
         connection = await factory.CreateConnectionAsync(cancellationToken);
         return HealthCheckResult.Healthy("RabbitMQ is healthy.");
      }
      catch (Exception e)
      {
         return HealthCheckResult.Unhealthy("RabbitMQ is unhealthy.", e);
      }
      finally
      {
         if (connection is not null)
         {
            await connection.CloseAsync(cancellationToken);
         }
      }
   }
}