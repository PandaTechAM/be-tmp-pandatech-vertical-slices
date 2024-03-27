using Pandatech.VerticalSlices.Infrastructure.Extensions;
using Pandatech.VerticalSlices.Infrastructure.Repositories;
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
         .AddRedisCache()
         .AddMassTransit(typeof(Program).Assembly)
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
