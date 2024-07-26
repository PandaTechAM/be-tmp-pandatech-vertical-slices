using MediatR;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.Features.Auth.Application.Auth;

namespace Pandatech.VerticalSlices.Features.Auth.Helpers.ApiAuth.MinimalApiExtensions;

public static class MinimalApiAuthExtensions
{
   public static RouteHandlerBuilder Authorize(this RouteHandlerBuilder builder,
      UserRole minimalUserRole = UserRole.Admin)
   {
      builder.Add(endpointBuilder =>
      {
         var original = endpointBuilder.RequestDelegate;

         endpointBuilder.RequestDelegate = async context =>
         {
            var forceToChangePassword =
               context.GetEndpoint()
                      ?.Metadata
                      .GetMetadata<ForcedPasswordChangeMetadata>() != null;
            var ignoreClientType = context.GetEndpoint()
                                          ?.Metadata
                                          .GetMetadata<IgnoreClientTypeMetadata>() != null;
            var sender = context.RequestServices.GetRequiredService<ISender>();


            await sender.Send(new AuthQuery(context,
                  minimalUserRole,
                  false,
                  forceToChangePassword,
                  ignoreClientType),
               context.RequestAborted);


            await original!(context);
            // Post-execution logic
         };
      });

      return builder;
   }
   public static RouteHandlerBuilder ForcedPasswordChange(this RouteHandlerBuilder builder)
   {
      builder.WithMetadata(new ForcedPasswordChangeMetadata());
      return builder;
   }

   public static RouteHandlerBuilder IgnoreClientType(this RouteHandlerBuilder builder)
   {
      builder.WithMetadata(new IgnoreClientTypeMetadata());
      return builder;
   }
}