using Carter;
using MediatR;
using Pandatech.VerticalSlices.Features.Auth.Helpers;
using Pandatech.VerticalSlices.Features.User.Application.Create;
using Pandatech.VerticalSlices.SharedKernel.Helpers;
using ResponseCrafter.Dtos;

namespace Pandatech.VerticalSlices.Features.User.Api;

public class UserV1Endpoints : ICarterModule
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

      groupApp.MapPost("/new", async (ISender mediator, CreateUserV1Command command) =>
         {
            await mediator.Send(command);
            return Results.Ok();
         })
         .Authorize()
         .Produces<ErrorResponse>(400);
   }
}
