using FluentMinimalApiMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pandatech.VerticalSlices.Features.Auth.Helpers.ApiAuth.MinimalApiExtensions;
using Pandatech.VerticalSlices.Features.User.Application.Create;
using Pandatech.VerticalSlices.Features.User.Application.Delete;
using Pandatech.VerticalSlices.Features.User.Application.GetColumnDistinctValues;
using Pandatech.VerticalSlices.Features.User.Application.GetUser;
using Pandatech.VerticalSlices.Features.User.Application.GetUsers;
using Pandatech.VerticalSlices.Features.User.Application.Update;
using Pandatech.VerticalSlices.Features.User.Application.UpdatePassword;
using Pandatech.VerticalSlices.Features.User.Application.UpdateStatus;
using Pandatech.VerticalSlices.SharedKernel.Helpers;
using ResponseCrafter.Extensions;
using QueryableExtensions = GridifyExtensions.Extensions.QueryableExtensions;

namespace Pandatech.VerticalSlices.Features.User.Api;

public class UserEndpoints : IEndpoint
{
    private const string BaseRoute = "/users";
    private const string TagName = "users";
    private static string RoutePrefix => ApiHelper.GetRoutePrefix(1, BaseRoute);

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var groupApp = app
            .MapGroup(RoutePrefix)
            .WithTags(TagName)
            .WithGroupName(ApiHelper.GroupVertical);

        groupApp.MapPost("",
                async (ISender sender, [FromBody] CreateUserCommand command, CancellationToken ct) =>
                {
                    await sender.Send(command, ct);
                    return TypedResults.Ok();
                })
            .Authorize()
            .ProducesBadRequest();

        groupApp.MapGet("/{id}",
                async (ISender sender, long id, CancellationToken ct) =>
                {
                    var user = await sender.Send(new GetUserQuery(id), ct);
                    return TypedResults.Ok(user);
                })
            .Authorize()
            .ProducesNotFound();


        groupApp.MapPut("/{id}",
                async (ISender sender,
                    long id,
                    [FromBody] UpdateUserCommand command,
                    CancellationToken ct) =>
                {
                    command.Id = id;
                    await sender.Send(command, ct);
                    return TypedResults.Ok();
                })
            .Authorize()
            .ProducesBadRequest()
            .ProducesConflict();


        groupApp.MapPatch("/{id}/password",
                async (ISender sender,
                    long id,
                    [FromBody] UpdateUserPasswordCommand command,
                    CancellationToken ct) =>
                {
                    command.Id = id;
                    await sender.Send(command, ct);
                    return TypedResults.Ok();
                })
            .Authorize()
            .ProducesBadRequest()
            .ProducesNotFound();

        groupApp.MapPatch("/{id}/status",
                async (ISender sender, long id, [FromBody] UpdateUserStatusCommand command, CancellationToken ct) =>
                {
                    command.Id = id;
                    await sender.Send(command, ct);
                    return TypedResults.Ok();
                })
            .Authorize()
            .ProducesBadRequest()
            .ProducesNotFound();

        groupApp.MapDelete("",
                async (ISender sender, [FromBody] DeleteUsersCommand command, CancellationToken ct) =>
                {
                    await sender.Send(command, ct);
                    return TypedResults.Ok();
                })
            .Authorize()
            .ProducesBadRequest();

        groupApp.MapGet("",
                async ([AsParameters] GetUsersQuery request, ISender sender, CancellationToken ct) =>
                {
                    var users = await sender.Send(request, ct);
                    return TypedResults.Ok(users);
                })
            .Authorize()
            .ProducesBadRequest();

        groupApp.MapGet("/column-distinct-values",
                async ([AsParameters] GetUserColumnDistinctValuesQuery query,
                    ISender sender,
                    CancellationToken ct) =>
                {
                    var distinctValues = await sender.Send(query, ct);
                    return TypedResults.Ok(distinctValues);
                })
            .Authorize()
            .ProducesBadRequest();

        groupApp.MapGet("/filters", () => TypedResults.Ok(QueryableExtensions.GetMappings<Domain.Entities.User>()))
            .Authorize()
            .WithSummary("Get filter technical information")
            .ProducesBadRequest();
    }
}
