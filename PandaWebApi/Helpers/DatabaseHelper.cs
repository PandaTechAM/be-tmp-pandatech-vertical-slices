using Microsoft.EntityFrameworkCore;

namespace PandaWebApi.Helpers;

public class DatabaseHelper
{
    private readonly IServiceProvider _serviceProvider;
    private readonly WebApplicationBuilder _builder;

    public DatabaseHelper(IServiceProvider serviceProvider, WebApplicationBuilder builder, ForTempTests tempTest)
    {
        _serviceProvider = serviceProvider;
        _builder = builder;
    }

    public string ResetDatabase<T>() where T : DbContext
    {
        try
        {
            if (_builder.Environment.IsDevelopment() || _builder.Environment.IsEnvironment("Local"))
            {
                return "Database reset is not allowed outside of Development environment!";
            }

            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<T>();
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