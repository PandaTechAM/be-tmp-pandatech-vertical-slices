using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PandaTech.IEnumerableFilters.PostgresContext;
using PandaWebApi.Models;

namespace PandaWebApi.Contexts;

public class PostgresContext : PostgresDbContext
{
    public PostgresContext(DbContextOptions<PostgresContext> options) : base(options)
    {
    }
    
}


public class PostgresContextFactory : IDesignTimeDbContextFactory<PostgresContext>{
    public PostgresContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PostgresContext>();
        
        optionsBuilder.UseNpgsql().UseSnakeCaseNamingConvention();
        
        return new PostgresContext(optionsBuilder.Options);
    }
}