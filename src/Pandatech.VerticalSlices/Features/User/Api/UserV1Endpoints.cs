using BaseConverter.Attributes;
using FluentMinimalApiMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pandatech.VerticalSlices.Features.Auth.Helpers;
using Pandatech.VerticalSlices.Features.User.Application.Create;
using Pandatech.VerticalSlices.Features.User.Application.Delete;
using Pandatech.VerticalSlices.Features.User.Application.GetById;
using Pandatech.VerticalSlices.Features.User.Application.Update;
using Pandatech.VerticalSlices.Features.User.Application.UpdatePassword;
using Pandatech.VerticalSlices.Features.User.Application.UpdateStatus;
using Pandatech.VerticalSlices.Features.User.Contracts.GetById;
using Pandatech.VerticalSlices.SharedKernel.Helpers;
using ResponseCrafter.Dtos;

namespace Pandatech.VerticalSlices.Features.User.Api;

public class UserV1Endpoints : IEndpoint
{
   private static string RoutePrefix => ApiHelper.GetRoutePrefix(1, BaseRoute);
   private const string BaseRoute = "/users";
   private const string TagName = "users";

   public void AddRoutes(IEndpointRouteBuilder app)
   {
      var groupApp = app
         .MapGroup(RoutePrefix)
         .WithTags(TagName)
         .WithGroupName(ApiHelper.GroupNameMain)
         .WithOpenApi();

      groupApp.MapPost("", async (ISender mediator, [FromBody] CreateUserV1Command command) =>
         {
            await mediator.Send(command);
            return Results.Ok();
         })
         .Authorize()
         .Produces(200)
         .Produces<ErrorResponse>(400);

      groupApp.MapGet("/{id}", async (ISender mediator, [PandaParameterBaseConverter] long id) =>
         {
            var user = await mediator.Send(new GetUserByIdV1Query(id));
            return Results.Ok(user);
         })
         .Authorize()
         .Produces<GetUserByIdV1QueryResponse>()
         .Produces<ErrorResponse>(404);


      groupApp.MapPut("/{id}",
            async (ISender mediator, [PandaParameterBaseConverter] long id, [FromBody] UpdateUserV1Command command) =>
            {
               command.Id = id;
               await mediator.Send(command);
               return Results.Ok();
            })
         .Authorize()
         .Produces(200)
         .Produces<ErrorResponse>(400)
         .Produces<ErrorResponse>(409);


      groupApp.MapPatch("/{id}/password",
            async (ISender mediator, [PandaParameterBaseConverter] long id,
               [FromBody] UpdateUserPasswordV1Command command) =>
            {
               command.Id = id;
               await mediator.Send(command);
               return Results.Ok();
            })
         .Authorize()
         .Produces(200)
         .Produces<ErrorResponse>(400)
         .Produces<ErrorResponse>(404);

      groupApp.MapPatch("/{id}/status",
            async (ISender mediator, [PandaParameterBaseConverter] long id,
               [FromBody] UpdateUserStatusV1Command command) =>
            {
               command.Id = id;
               await mediator.Send(command);
               return Results.Ok();
            })
         .Authorize()
         .Produces(200)
         .Produces<ErrorResponse>(400)
         .Produces<ErrorResponse>(404);

      groupApp.MapDelete("",
            async (ISender mediator, [FromBody] DeleteUsersV1Command command) =>
            {
               await mediator.Send(command);
               return Results.Ok();
            })
         .Authorize()
         .Produces(200)
         .Produces<ErrorResponse>(400);
   }
}
