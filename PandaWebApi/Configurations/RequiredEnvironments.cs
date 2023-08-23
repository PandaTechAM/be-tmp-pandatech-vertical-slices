namespace PandaWebApi.Configurations;

public static class RequiredEnvironments
{
    public static IEnumerable<string> GetEnvironmentVariables()
    {
        List<string> environmentVariables = new()
        {
            "POSTGRES_CONNECTION_STRING",
            "REDIS_CONNECTION_STRING",
            "RABBITMQ_EXCHANGE_NAME",
            "RABBITMQ_ROUTING_KEY",
            "RABBITMQ_QUEUE_NAME",
            "RABBITMQ_ROUTING_KEY_DLX",
            "RABBITMQ_QUEUE_NAME_DLX",
            "RABBITMQ_URI"
        };

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
#pragma warning disable S108
        {
        }
#pragma warning restore S108

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Staging")
        {
            environmentVariables.Add("ELASTIC_SEARCH_URL");
            environmentVariables.Add("CERTIFICATE");
            environmentVariables.Add("CERTIFICATE_KEY");
        }

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
        {
            environmentVariables.Add("ELASTIC_SEARCH_URL");
            environmentVariables.Add("CERTIFICATE");
            environmentVariables.Add("CERTIFICATE_KEY");
        }

        return environmentVariables;
    }
}