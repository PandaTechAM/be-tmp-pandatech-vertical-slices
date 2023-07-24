using Serilog;

namespace WebApiPostgres.Configurations;

public abstract class ConfigureLogger
{
    public static void AddSerilogWithElastic(WebApplicationBuilder builder)
    {
#if DEBUG
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .WriteTo.Console()
            .CreateLogger();
#else
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(Environment.GetEnvironmentVariable("ELASTIC_SEARCH_URL"),
                indexFormat: $"be-tmp-web-api-postgres-logs-{DateTime.UtcNow:yyyy.MM}", autoRegisterTemplate: true,
                detectElasticsearchVersion: true,
                numberOfShards: 5,
                numberOfReplicas: 1,
                bufferBaseFilename: "./be-tmp-web-api-postgres-logs/elastic-buffer",
                bufferFileSizeLimitBytes: 1024 * 1024 * 32); //32 MB each buffer file
            .CreateLogger();
#endif

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();
        builder.Services.AddSingleton(Log.Logger);
    }
}