using Microsoft.EntityFrameworkCore;
using WebApiPostgres.Contexts;

namespace WebApiPostgres.Configurations;

public abstract class SetupDatabases
{
    public static void AddPostgresContext(WebApplicationBuilder builder)
    {
        var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
        
        builder.Services.AddDbContextPool<PostgresContext>(options =>
            options.UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention());

    }
}