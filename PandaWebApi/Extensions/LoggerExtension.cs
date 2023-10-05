using Serilog;

namespace PandaWebApi.Extensions;

public static class LoggerExtension
{
    public static void AddSerilog(this WebApplicationBuilder builder)
    {
        var indexName = Environment.GetEnvironmentVariable("ELASTIC_INDEX_NAME");
        var elasticSearchUrl = Environment.GetEnvironmentVariable("ELASTIC_SEARCH_URL");

        //This part is for general configuration
        var loggerConfig = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName();


        //This part is for "DEVELOPMENT" environment
        if (builder.Environment.IsDevelopment())
        {
            loggerConfig.MinimumLevel.Information()
                .WriteTo.Console();
        }

        //This part is for "STAGING" environment
        if (builder.Environment.IsStaging())
        {
            loggerConfig.MinimumLevel.Information()
                
                .WriteTo.Elasticsearch(elasticSearchUrl,
                    indexFormat: $"{indexName}-logs-{DateTime.UtcNow:yyyy.MM}", 
                    autoRegisterTemplate: true,
                    detectElasticsearchVersion: true,
                    numberOfShards: 5,
                    numberOfReplicas: 1,
                    bufferBaseFilename: $"./{indexName}-logs/elastic-buffer",
                    bufferFileSizeLimitBytes: 1024 * 1024 * 32); //32 MB each buffer file
        }

        //This part is for "PRODUCTION" environment
        if (builder.Environment.IsProduction())
        {
            loggerConfig.MinimumLevel.Information()
                
                .WriteTo.Elasticsearch(elasticSearchUrl,
                    indexFormat: $"{indexName}-logs-{DateTime.UtcNow:yyyy.MM}", 
                    autoRegisterTemplate: true,
                    detectElasticsearchVersion: true,
                    numberOfShards: 5,
                    numberOfReplicas: 1,
                    bufferBaseFilename: $"./{indexName}-logs/elastic-buffer",
                    bufferFileSizeLimitBytes: 1024 * 1024 * 32); //32 MB each buffer file
        }

        Log.Logger = loggerConfig.CreateLogger();
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();
        builder.Services.AddSingleton(Log.Logger);
    }
}