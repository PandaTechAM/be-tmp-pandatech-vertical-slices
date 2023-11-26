using Microsoft.EntityFrameworkCore;
using PandaWebApi.Contexts;

namespace PandaWebApi.Extensions;

public static class DatabaseExtension
{
    public static void AddPostgresContext(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services.AddDbContextPool<PostgresContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Postgres"))
                .UseSnakeCaseNamingConvention());
    }

    public static void MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PostgresContext>();
        dbContext.Database.Migrate();
    }
}