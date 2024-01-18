using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Filters;
using PandaWebApi.Contexts;
using PandaWebApi.DTOs.User;
using PandaWebApi.Enums;
using PandaWebApi.Services.Interfaces;
using ResponseCrafter.StandardHttpExceptions;

namespace PandaWebApi.Attributes;

public class AuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly Roles _minimalRoleLevel;

    public AuthorizeAttribute()
    {
        _minimalRoleLevel = Roles.Admin;
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
        
        var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var tokenService = context.HttpContext.RequestServices.GetRequiredService<IUserTokenService>();
        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();

        var token = await tokenService.ValidateTokenAsync(context.HttpContext);

        var user = token.User;
        if (user.Role > _minimalRoleLevel)
            throw new ForbiddenException();

        var isActionAllowed = context.ActionDescriptor.EndpointMetadata
            .Any(em => em.GetType() == typeof(AllowPasswordChange));

        if (user.ForcePasswordChange && !isActionAllowed)
        {
            throw new ForbiddenException("change_password_required");
        }
        
        userService.SetUserContext(token, contextUser);
    }
}