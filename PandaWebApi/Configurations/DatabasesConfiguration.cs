using Microsoft.EntityFrameworkCore;
using PandaWebApi.Contexts;

namespace PandaWebApi.Configurations;

public static class DatabasesConfiguration
{
    public static void AddPostgresContext(this WebApplicationBuilder builder)
    {
        var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
        
        builder.Services.AddDbContextPool<PostgresContext>(options =>
            options.UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention());
    }
}