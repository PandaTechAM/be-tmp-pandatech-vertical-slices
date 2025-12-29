using FluentMinimalApiMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.Features.Auth.Helpers.ApiAuth.MinimalApiExtensions;
using Pandatech.VerticalSlices.Features.UserConfig.Application.CreateOrUpdate;
using Pandatech.VerticalSlices.Features.UserConfig.Application.Delete;
using Pandatech.VerticalSlices.Features.UserConfig.Application.Get;
using Pandatech.VerticalSlices.SharedKernel.Helpers;
using ResponseCrafter.Extensions;

namespace Pandatech.VerticalSlices.Features.UserConfig.Api;

public class UserConfigEndpoints : IEndpoint
{
   private const string BaseRoute = "/user";
   private const string TagName = "user-configs";
   private static string RoutePrefix => ApiHelper.GetRoutePrefix(1, BaseRoute);

   public void AddRoutes(IEndpointRouteBuilder app)
   {
      var groupApp = app
                     .MapGroup(RoutePrefix)
                     .WithTags(TagName)
                     .WithGroupName(ApiHelper.GroupVertical);

      groupApp.MapPost("/frontend/configs",
                 async ([FromBody] CreateOrUpdateUserConfigCommand request,
                    [FromServices] ISender sender,
                    CancellationToken ct) =>
                 {
                    await sender.Send(request, ct);
                    return TypedResults.Ok();
                 })
              .WithSummary("Create or update user frontend configs")
              .Authorize(UserRole.User)
              .ProducesBadRequest();

      groupApp.MapGet("/frontend/configs",
                 async ([AsParameters] GetUserConfigsQuery query,
                    [FromServices] ISender sender,
                    CancellationToken ct) =>
                 {
                    var configs = await sender.Send(query, ct);
                    return TypedResults.Ok(configs);
                 })
              .WithSummary("Get user frontend configs")
              .Authorize(UserRole.User)
              .ProducesBadRequest();

      groupApp.MapDelete("/frontend/configs",
                 async ([FromBody] DeleteUserConfigsCommand request,
                    [FromServices] ISender sender,
                    CancellationToken ct) =>
                 {
                    await sender.Send(request, ct);
                    return TypedResults.Ok();
                 })
              .WithSummary("Delete user frontend configs")
              .Authorize(UserRole.User)
              .ProducesBadRequest();
   }
}