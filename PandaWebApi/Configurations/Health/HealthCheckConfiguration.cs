using RabbitMQ.Client;

namespace PandaWebApi.Configurations.Health;

public static class HealthCheckConfiguration
{
    public static void AddHealthChecks(this WebApplicationBuilder builder)
    {
        var timeoutSeconds = TimeSpan.FromSeconds(5);

        //This part is only for RMQ health check
        ConnectionFactory factory = new()
        {
            Uri = new Uri(Environment.GetEnvironmentVariable("RABBITMQ_URI")!),
        };
        var connection = factory.CreateConnection();


        if (builder.Environment.IsDevelopment())
        {
            builder.Services
                .AddSingleton(connection)
                .AddHealthChecks()
                .AddNpgSql(Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING")!, timeout: timeoutSeconds)
                .AddRedis(Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING")!, timeout: timeoutSeconds)
                .AddRabbitMQ();
        }

        if (builder.Environment.IsStaging())
        {
            builder.Services
                .AddSingleton(connection)
                .AddHealthChecks()
                .AddCheck<UserManagementHealthCheck>("UserManagementHealthCheck", timeout: timeoutSeconds)
                .AddNpgSql(Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING")!, timeout: timeoutSeconds)
                .AddRedis(Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING")!, timeout: timeoutSeconds)
                .AddElasticsearch(Environment.GetEnvironmentVariable("ELASTIC_SEARCH_URL")!, timeout: timeoutSeconds)
                .AddRabbitMQ();
        }

        if (builder.Environment.IsProduction())
        {
            builder.Services
                .AddSingleton(connection)
                .AddHealthChecks()
                .AddCheck<UserManagementHealthCheck>("UserManagementHealthCheck", timeout: timeoutSeconds)
                .AddNpgSql(Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING")!, timeout: timeoutSeconds)
                .AddRedis(Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING")!, timeout: timeoutSeconds)
                .AddElasticsearch(Environment.GetEnvironmentVariable("ELASTIC_SEARCH_URL")!, timeout: timeoutSeconds)
                .AddRabbitMQ();
        }
    }
}