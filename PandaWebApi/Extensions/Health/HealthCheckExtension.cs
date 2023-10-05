using RabbitMQ.Client;

namespace PandaWebApi.Extensions.Health;

public static class HealthCheckExtension
{
    public static void AddHealthChecks(this WebApplicationBuilder builder)
    {
        var timeoutSeconds = TimeSpan.FromSeconds(5);
        var postgresConnectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING")!;
        var redisConnectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING")!;
        var elasticSearchUrl = Environment.GetEnvironmentVariable("ELASTIC_SEARCH_URL")!;
        var rabbitMqUri = Environment.GetEnvironmentVariable("RABBITMQ_URI")!;

        //This part is only for RMQ health check
        ConnectionFactory factory = new()
        {
            Uri = new Uri(rabbitMqUri)
        };
        var connection = factory.CreateConnection();


        if (builder.Environment.IsDevelopment())
        {
            builder.Services
                .AddSingleton(connection)
                .AddHealthChecks()
                .AddNpgSql(postgresConnectionString, timeout: timeoutSeconds)
                .AddRedis(redisConnectionString, timeout: timeoutSeconds)
                .AddRabbitMQ();
        }

        if (builder.Environment.IsStaging())
        {
            builder.Services
                .AddSingleton(connection)
                .AddHealthChecks()
                .AddCheck<UserManagementHealthCheck>("UserManagementHealthCheck", timeout: timeoutSeconds)
                .AddNpgSql(postgresConnectionString, timeout: timeoutSeconds)
                .AddRedis(redisConnectionString, timeout: timeoutSeconds)
                .AddElasticsearch(elasticSearchUrl, timeout: timeoutSeconds)
                .AddRabbitMQ();
        }

        if (builder.Environment.IsProduction())
        {
            builder.Services
                .AddSingleton(connection)
                .AddHealthChecks()
                .AddCheck<UserManagementHealthCheck>("UserManagementHealthCheck", timeout: timeoutSeconds)
                .AddNpgSql(postgresConnectionString, timeout: timeoutSeconds)
                .AddRedis(redisConnectionString, timeout: timeoutSeconds)
                .AddElasticsearch(elasticSearchUrl, timeout: timeoutSeconds)
                .AddRabbitMQ();
        }
    }
}