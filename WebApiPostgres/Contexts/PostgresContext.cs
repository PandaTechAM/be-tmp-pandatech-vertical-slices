using Microsoft.EntityFrameworkCore;

namespace WebApiPostgres.Contexts;

public class PostgresContext : DbContext
{
    public PostgresContext(DbContextOptions<PostgresContext> options) : base(options)
    {
    }
    
}