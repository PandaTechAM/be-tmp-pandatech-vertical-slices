using Pandatech.VerticalSlices.Features.Auth.Contracts.Authenticate;
using Pandatech.VerticalSlices.SharedKernel.Helpers;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;
using PandaVaultClient;

namespace Pandatech.VerticalSlices.SharedKernel.Extensions;

public static class RegisterServicesExtensions
{
   public static WebApplicationBuilder RegisterAllServices(this WebApplicationBuilder builder)
   {
      builder.AddServices();
      builder.RegisterPandaVaultEndpoint(); //optional


      return builder;
   }

   private static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
   {
      if (builder.Environment.IsLocal())
      {
         builder.Services.AddSingleton<DatabaseHelper>();
      }


      builder.Services.AddScoped<IRequestContext, RequestContext>();
      return builder;
   }
}
