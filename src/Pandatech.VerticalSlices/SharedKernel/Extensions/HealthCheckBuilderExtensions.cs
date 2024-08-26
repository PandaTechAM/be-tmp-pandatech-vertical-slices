using Pandatech.VerticalSlices.SharedKernel.Helpers;
using RabbitMQ.Client;

namespace Pandatech.VerticalSlices.SharedKernel.Extensions;

public static class HealthCheckBuilderExtensions
{
   public static WebApplicationBuilder AddHealthChecks(this WebApplicationBuilder builder)
   {
      var configuration = builder.Configuration;
      var timeoutSeconds = TimeSpan.FromSeconds(5);
      var postgresConnectionString = configuration.GetPostgresUrl();
      var redisConnectionString = configuration.GetRedisUrl();
      var rabbitMqUri = configuration.GetRabbitMqUrl();

      //This part is only for RMQ health check
      ConnectionFactory factory = new()
      {
         Uri = new Uri(rabbitMqUri)
      };
      var connection = factory.CreateConnection();


      if (builder.Environment.IsLocal())
      {
         builder.Services
                .AddSingleton(connection)
                .AddHealthChecks()
                .AddRabbitMQ(name: "rabbit_mq")
                .AddNpgSql(postgresConnectionString, timeout: timeoutSeconds, name: "postgres")
                .AddRedis(redisConnectionString, timeout: timeoutSeconds);
      }

      else if (builder.Environment.IsProduction())
      {
         builder.Services
                .AddSingleton(connection)
                .AddHealthChecks()
                .AddNpgSql(postgresConnectionString, timeout: timeoutSeconds, name: "postgres")
                .AddRedis(redisConnectionString, timeout: timeoutSeconds)
                .AddRabbitMQ();
      }
      else
      {
         builder.Services
                .AddSingleton(connection)
                .AddHealthChecks()
                .AddNpgSql(postgresConnectionString, timeout: timeoutSeconds, name: "postgres")
                .AddRedis(redisConnectionString, timeout: timeoutSeconds)
                .AddRabbitMQ();
      }

      return builder;
   }
}