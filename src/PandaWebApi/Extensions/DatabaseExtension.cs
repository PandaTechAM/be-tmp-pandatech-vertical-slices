using Microsoft.EntityFrameworkCore;
using PandaWebApi.Contexts;

namespace PandaWebApi.Extensions;

public static class DatabaseExtension
{
    public static WebApplicationBuilder AddPostgresContext(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        builder.Environment.IsDevelopment();

        builder.Services.AddDbContextPool<PostgresContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Postgres"))
                .UseSnakeCaseNamingConvention());
        return builder;
    }

    public static WebApplication MigrateDatabase(this WebApplication app)
    {
        app.Environment.IsDevelopment();
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PostgresContext>();
        dbContext.Database.Migrate();
        return app;
    }
}