using FluentMinimalApiMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.Features.Auth.Application.IdentityState;
using Pandatech.VerticalSlices.Features.Auth.Application.Login;
using Pandatech.VerticalSlices.Features.Auth.Application.RefreshToken;
using Pandatech.VerticalSlices.Features.Auth.Application.UpdatePasswordForced;
using Pandatech.VerticalSlices.Features.Auth.Helpers;
using Pandatech.VerticalSlices.Features.Auth.Helpers.ApiAuth.MinimalApiExtensions;
using Pandatech.VerticalSlices.Features.MyAccount.Application.RevokeCurrentToken;
using Pandatech.VerticalSlices.Features.MyAccount.Application.UpdateOwnPassword;
using Pandatech.VerticalSlices.SharedKernel.Enums;
using Pandatech.VerticalSlices.SharedKernel.Helpers;
using ResponseCrafter.Extensions;

namespace Pandatech.VerticalSlices.Features.Auth.Api;

public class AuthenticationEndpoints : IEndpoint
{
   private const string BaseRoute = "/authentication";
   private const string TagName = "authentication";
   private static string RoutePrefix => ApiHelper.GetRoutePrefix(1, BaseRoute);

   public void AddRoutes(IEndpointRouteBuilder app)
   {
      var groupApp = app
         .MapGroup(RoutePrefix)
         .WithTags(TagName)
         .WithGroupName(ApiHelper.GroupVertical)
         .WithOpenApi();


      groupApp.MapPost("/login",
            async (ISender sender, LoginCommand command, IHttpContextAccessor httpContextAccessor,
               IHostEnvironment environment, IConfiguration configuration, CancellationToken token) =>
            {
               var response = await sender.Send(command, token);
               var clientType = httpContextAccessor.HttpContext!.TryParseClientType().ConvertToEnum();

               if (clientType != ClientType.Browser)
               {
                  return TypedResults.Ok(response);
               }

               var domain = configuration["Security:CookieDomain"]!;
               httpContextAccessor.HttpContext!.PrepareAndSetCookies(response, environment, domain);

               return TypedResults.Ok(response);
            })
         .WithSummary(" \ud83c\udf6a Cookies for the browser and token for the rest of the clients. \ud83c\udf6a")
         .WithDescription(
            "This endpoint is used to authenticate a user. Be aware that the response will be different depending on the client type.")
         .ProducesErrorResponse(400);


      groupApp.MapPost("/refresh-token",
            async (ISender sender, IHttpContextAccessor httpContextAccessor,
               IHostEnvironment environment, IConfiguration configuration, CancellationToken token) =>
            {
               var refreshTokenSignature = httpContextAccessor.HttpContext!.TryParseRefreshTokenSignature(environment);
               var response = await sender.Send(new RefreshTokenCommand(refreshTokenSignature), token);
               var clientType = httpContextAccessor.HttpContext!.TryParseClientType().ConvertToEnum();

               if (clientType != ClientType.Browser)
               {
                  return TypedResults.Ok(response);
               }

               var domain = configuration["Security:CookieDomain"]!;
               httpContextAccessor.HttpContext!.PrepareAndSetCookies(response, environment, domain);

               return TypedResults.Ok(response);
            })
         .WithSummary(" \ud83c\udf6a Cookies for the browser and token for the rest of the clients. \ud83c\udf6a")
         .WithDescription("This endpoint is used to refresh the user token.")
         .ProducesErrorResponse(400);


      groupApp.MapGet("/state", async (ISender sender, CancellationToken token) =>
         {
            var identity = await sender.Send(new GetIdentityStateQuery(), token);
            return TypedResults.Ok(identity);
         })
         .Authorize(UserRole.User)
         .WithDescription("This endpoint is used to get the current user state.");



      groupApp.MapPatch("/password/force",
            async (ISender sender, UpdatePasswordForcedCommand command, CancellationToken token) =>
            {
               await sender.Send(command, token);
               return TypedResults.Ok();
            })
         .Authorize(UserRole.User)
         .ForcedPasswordChange()
         .WithDescription("This endpoint is used to update the user password when it is forced.")
         .ProducesErrorResponse(400);
   }
}
