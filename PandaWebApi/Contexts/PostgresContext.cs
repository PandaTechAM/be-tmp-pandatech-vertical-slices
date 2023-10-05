using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PandaWebApi.Models;

namespace PandaWebApi.Contexts;

public class PostgresContext : DbContext
{
    public PostgresContext(DbContextOptions<PostgresContext> options) : base(options)
    {
    }
    
    public DbSet<SomeModel> SomeModels { get; set; } = null!;
}

public class PostgresContextFactory : IDesignTimeDbContextFactory<PostgresContext>{
    public PostgresContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PostgresContext>();
        
        optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING"));
        
        return new PostgresContext(optionsBuilder.Options);
    }
} 