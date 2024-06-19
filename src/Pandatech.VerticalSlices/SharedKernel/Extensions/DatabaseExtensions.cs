using EFCore.PostgresExtensions.Extensions;
using Microsoft.EntityFrameworkCore;
using Pandatech.VerticalSlices.Context;
using Pandatech.VerticalSlices.SharedKernel.Helpers;

namespace Pandatech.VerticalSlices.SharedKernel.Extensions;

public static class DatabaseExtensions
{
   public static WebApplicationBuilder AddPostgresContext(this WebApplicationBuilder builder)
   {
      var configuration = builder.Configuration;

      var connectionString = configuration.GetConnectionString(ConfigurationPaths.PostgresUrl);
      builder.Services.AddDbContextPool<PostgresContext>(options =>
         options.UseNpgsql(connectionString)
            .UseQueryLocks()
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
