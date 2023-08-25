using Microsoft.EntityFrameworkCore;
using PandaWebApi.Contexts;

namespace PandaWebApi.Services.Implementations;

public class DatabaseReset
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseReset(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public string ResetDatabase()
    {
        try
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
            {
                return "Database reset is not allowed outside of Development environment!";
            }

            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<PostgresContext>();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.Migrate();
        }
        catch (Exception e)
        {
            return $"Database has not been reset due to following error: {e.Message}";
        }

        return "Database reset success!";
    }
}