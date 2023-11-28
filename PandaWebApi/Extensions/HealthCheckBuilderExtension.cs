using Microsoft.Extensions.Diagnostics.HealthChecks;
using PandaWebApi.Helpers;
using RabbitMQ.Client;

namespace PandaWebApi.Extensions;

public static class HealthCheckBuilderExtension
{
    public static WebApplicationBuilder AddHealthChecks(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        var timeoutSeconds = TimeSpan.FromSeconds(5);
        var postgresConnectionString = configuration.GetConnectionString("Postgres")!;
        var redisConnectionString = configuration.GetConnectionString("Redis")!;
        var elasticSearchUrl = configuration.GetConnectionString("ElasticSearch")!;
        var rabbitMqUri = configuration.GetConnectionString("RabbitMQ")!;
        var userManagementHealthCheck = new CustomHealthChecks(configuration.GetConnectionString("AuditTrail")!);

        //This part is only for RMQ health check
        ConnectionFactory factory = new()
        {
            Uri = new Uri(rabbitMqUri)
        };
        var connection = factory.CreateConnection();


        if (builder.Environment.IsEnvironment("Local"))
        {
            builder.Services
                .AddSingleton(connection)
                .AddHealthChecks()
                .AddRabbitMQ()
                .AddNpgSql(postgresConnectionString, timeout: timeoutSeconds, name: "postgres")
                .AddCheck("AuditTrail", userManagementHealthCheck, timeout: timeoutSeconds, failureStatus: HealthStatus.Degraded)
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
                .AddCheck("AuditTrail", userManagementHealthCheck, timeout: timeoutSeconds, failureStatus: HealthStatus.Degraded)
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
                .AddCheck("AuditTrail", userManagementHealthCheck, timeout: timeoutSeconds, failureStatus: HealthStatus.Degraded)
                .AddRabbitMQ();
        }

        return builder;
    }
}