using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PandaWebApi.Contexts;
using PandaWebApi.DTOs;
using PandaWebApi.Enums;
using PandaWebApi.Services.Interfaces;
using ResponseCrafter.StandardHttpExceptions;

namespace PandaWebApi.Attributes;

[SuppressMessage("ReSharper", "ConvertToPrimaryConstructor")]
public class AuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly Roles _minimalRoleLevel;

    public AuthorizeAttribute()
    {
        _minimalRoleLevel = Roles.User;
    }

    public AuthorizeAttribute(Roles role)
    {
        _minimalRoleLevel = role;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var isExcluded = context.ActionDescriptor.EndpointMetadata
            .Any(em => em.GetType() == typeof(Anonymous));

        if (isExcluded)
            return;

        var contextUser = context.HttpContext.RequestServices.GetRequiredService<ContextUser>();
        var dbContext = context.HttpContext.RequestServices.GetRequiredService<PostgresContext>();
        var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();
        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();

        var cookie = context.HttpContext.Request.Cookies["Token"];
        var token = await tokenService.ValidateTokenAsync(cookie, dbContext);

        var user = token.User;
        if (user.Role > _minimalRoleLevel)
            throw new ForbiddenException();

        var isActionAllowed = context.ActionDescriptor.EndpointMetadata
            .Any(em => em.GetType() == typeof(AllowPasswordChange));

        if (user.ForcePasswordChange && !isActionAllowed)
        {
            throw new ForbiddenException("change_password_required");
        }

        await tokenService.UpdateTokenExpirationAsync(token, configuration, dbContext);

        userService.SetUserContext(token, contextUser);
    }
}