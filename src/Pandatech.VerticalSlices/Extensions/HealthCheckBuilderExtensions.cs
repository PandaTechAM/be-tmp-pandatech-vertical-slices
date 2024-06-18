using Pandatech.VerticalSlices.SharedKernel.Extensions;
using Pandatech.VerticalSlices.SharedKernel.Helpers;
using RabbitMQ.Client;

namespace Pandatech.VerticalSlices.Extensions;

public static class HealthCheckBuilderExtensions
{
   public static WebApplicationBuilder AddHealthChecks(this WebApplicationBuilder builder)
   {
      var configuration = builder.Configuration;
      var timeoutSeconds = TimeSpan.FromSeconds(5);
      var postgresConnectionString = configuration.GetConnectionString(ConfigurationPaths.PostgresUrl)!;
      var redisConnectionString = configuration.GetConnectionString(ConfigurationPaths.RedisUrl)!;
      var elasticSearchUrl = configuration.GetConnectionString(ConfigurationPaths.ElasticSearchUrl)!;
      var rabbitMqUri = configuration.GetConnectionString(ConfigurationPaths.RabbitMqUrl)!;

      //This part is only for RMQ health check
      ConnectionFactory factory = new() { Uri = new Uri(rabbitMqUri) };
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
            .AddElasticsearch(elasticSearchUrl, timeout: timeoutSeconds)
            .AddRabbitMQ();
      }
      else
      {
         builder.Services
            .AddSingleton(connection)
            .AddHealthChecks()
            .AddNpgSql(postgresConnectionString, timeout: timeoutSeconds, name: "postgres")
            .AddRedis(redisConnectionString, timeout: timeoutSeconds)
            .AddElasticsearch(elasticSearchUrl, timeout: timeoutSeconds)
            .AddRabbitMQ();
      }

      return builder;
   }
}