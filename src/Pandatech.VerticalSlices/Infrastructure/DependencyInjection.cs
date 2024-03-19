using Pandatech.VerticalSlices.Infrastructure.Extensions;
using Pandatech.VerticalSlices.Infrastructure.Repositories;
using Pandatech.VerticalSlices.Infrastructure.Seed;
using Pandatech.VerticalSlices.Infrastructure.Seed.User;
using Pandatech.VerticalSlices.SharedKernel.Extensions;

namespace Pandatech.VerticalSlices.Infrastructure;

public static class DependencyInjection
{
   public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
   {
      builder.AddSerilog()
         .AddHangfireServer()
         .AddPostgresContext()
         .AddPandaCryptoAndFilters()
         .AddHealthChecks();

      builder.Services.AddScoped<SharedRepository>();
      return builder;
   }

   public static WebApplication UseInfrastructure(this WebApplication app)
   {
      app.MigrateDatabase()
         .EnsureHealthy()
         .UseHangfireServer()
         .SeedSystemUser();

      return app;
   }
}
