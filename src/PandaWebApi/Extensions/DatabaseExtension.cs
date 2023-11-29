using Microsoft.EntityFrameworkCore;
using PandaWebApi.Contexts;

namespace PandaWebApi.Extensions;

public static class DatabaseExtension
{
    public static WebApplicationBuilder AddPostgresContext(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        var connectionString = configuration.GetConnectionString("Postgres");
        builder.Services.AddDbContextPool<PostgresContext>(options =>
            options.UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention());
        return builder;
    }

    public static WebApplication MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PostgresContext>();
        dbContext.Database.Migrate();
        return app;
    }
}