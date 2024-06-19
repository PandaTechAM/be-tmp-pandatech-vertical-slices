using System.Reflection;
using MassTransit;
using Pandatech.VerticalSlices.SharedKernel.Helpers;

namespace Pandatech.VerticalSlices.SharedKernel.Extensions;

public static class MassTransitExtension
{
   public static WebApplicationBuilder AddMassTransit(this WebApplicationBuilder builder, params Assembly[] assemblies)
   {
      builder.Services.AddMassTransit(x =>
      {
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
