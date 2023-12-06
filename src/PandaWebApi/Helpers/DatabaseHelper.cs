using Microsoft.EntityFrameworkCore;

namespace PandaWebApi.Helpers;

public class DatabaseHelper
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseHelper(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public string ResetDatabase<T>() where T : DbContext
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<T>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.Migrate();

        return "Database reset success!";
    }
}