namespace PandaWebApi.Configurations;

public static class Environments
{
    public static IEnumerable<string> GetEnvironmentVariables()
    {
        var aspEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");


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

        switch (aspEnv)
        {
            case "Development":
                
                return environmentVariables;
            
            case "Staging":
                environmentVariables.Add("ELASTIC_SEARCH_URL");
                environmentVariables.Add("CERTIFICATE");
                environmentVariables.Add("CERTIFICATE_KEY");

                return environmentVariables;
            
            case "Production":
            {
                environmentVariables.Add("ELASTIC_SEARCH_URL");
                environmentVariables.Add("CERTIFICATE");
                environmentVariables.Add("CERTIFICATE_KEY");

                return environmentVariables;
            }
            default:
                throw new ArgumentException($"`ASPNETCORE_ENVIRONMENT` has value set to `{aspEnv}`, which is invalid value");
        }
    }
}