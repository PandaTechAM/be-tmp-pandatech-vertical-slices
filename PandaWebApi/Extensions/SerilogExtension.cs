using Serilog;

namespace PandaWebApi.Extensions;

public static class SerilogExtension
{
    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        var indexName = configuration["Serilog:ElasticIndexName"]!;
        var elasticSearchUrl = configuration.GetConnectionString("ElasticSearch")!;

        var loggerConfig = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .ReadFrom.Configuration(configuration);

        ConfigureEnvironmentSpecificSettings(builder.Environment, loggerConfig, elasticSearchUrl, indexName);

        Log.Logger = loggerConfig.CreateLogger();
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();
        builder.Services.AddSingleton(Log.Logger);
        return builder;
    }

    private static void ConfigureEnvironmentSpecificSettings(IHostEnvironment environment,
        LoggerConfiguration loggerConfig, string elasticSearchUrl, string indexName)
    {
        if (environment.IsEnvironment("Local"))
        {
            loggerConfig.WriteTo.Console();
        }
        else
        {
            ConfigureElasticsearch(loggerConfig, elasticSearchUrl, indexName);
        }
    }

    private static void ConfigureElasticsearch(LoggerConfiguration loggerConfig, string elasticSearchUrl,
        string indexName)
    {
        loggerConfig.WriteTo.Elasticsearch(elasticSearchUrl,
            indexFormat: $"{indexName}-logs-{DateTime.UtcNow:yyyy.MM}",
            autoRegisterTemplate: true,
            detectElasticsearchVersion: true,
            numberOfShards: 5,
            numberOfReplicas: 1,
            bufferBaseFilename: $"./{indexName}-logs/elastic-buffer",
            bufferFileSizeLimitBytes: 1024 * 1024 * 16); // 16 MB each buffer file
    }
}