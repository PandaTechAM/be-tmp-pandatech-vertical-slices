using RabbitMQ.Client;

namespace PandaWebApi.Configurations;

public static class HealthCheckConfiguration
{
    public static void AddHealthChecks(this WebApplicationBuilder builder)
    {
        ConnectionFactory factory = new()
        {
            Uri = new Uri(Environment.GetEnvironmentVariable("RABBITMQ_URI")!),
        };
        var connection = factory.CreateConnection();
        
        builder.Services
            .AddSingleton(connection)
            .AddHealthChecks()
            .AddNpgSql(Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING")!)
            .AddRedis(Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING")!)
            .AddElasticsearch(Environment.GetEnvironmentVariable("ELASTIC_SEARCH_URL")!)
            .AddRabbitMQ();
    }
}