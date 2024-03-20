using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Pandatech.VerticalSlices.Infrastructure.Contexts;

public class PostgresContextFactory : IDesignTimeDbContextFactory<PostgresContext>
{
   public PostgresContext CreateDbContext(string[] args)
   {
      var optionsBuilder = new DbContextOptionsBuilder<PostgresContext>();

      optionsBuilder.UseNpgsql().UseSnakeCaseNamingConvention();

      return new PostgresContext(optionsBuilder.Options);
   }
}
