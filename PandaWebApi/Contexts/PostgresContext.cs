using Microsoft.EntityFrameworkCore;
using PandaWebApi.Models;

namespace PandaWebApi.Contexts;

public class PostgresContext : DbContext
{
    public PostgresContext(DbContextOptions<PostgresContext> options) : base(options)
    {
    }
    
    public DbSet<SomeModel> SomeModels { get; set; } = null!;
}