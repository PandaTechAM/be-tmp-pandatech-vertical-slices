using System.Reflection;
using MassTransit;
using Pandatech.VerticalSlices.Infrastructure.Context;
using Pandatech.VerticalSlices.SharedKernel.Helpers;

namespace Pandatech.VerticalSlices.Infrastructure.Extensions;

public static class MassTransitExtension
{
   public static WebApplicationBuilder AddMassTransit(this WebApplicationBuilder builder, params Assembly[] assemblies)
   {
      builder.Services.AddMassTransit(x =>
      {
         x.AddEntityFrameworkOutbox<PostgresContext>(o =>
         {
            o.DuplicateDetectionWindow = TimeSpan.FromMinutes(25);
            o.QueryDelay = TimeSpan.FromMilliseconds(1500);
            o.UsePostgres().UseBusOutbox();
         });

         x.AddConsumers(assemblies);
         x.SetKebabCaseEndpointNameFormatter();


         x.UsingRabbitMq((context, cfg) =>
         {
            cfg.Host(builder.Configuration.GetConnectionString(ConfigurationPaths.RabbitMqUrl));
            cfg.ConfigureEndpoints(context);
            cfg.UseMessageRetry(r =>
               r.Exponential(5, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(2)));
         });
      });
      return builder;
   }
}
