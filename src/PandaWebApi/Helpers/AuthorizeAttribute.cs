using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using PandaWebApi.Contexts;
using PandaWebApi.DTOs.Authentication;
using PandaWebApi.Enums;
using ResponseCrafter.StandardHttpExceptions;

namespace PandaWebApi.Helpers;

public class AuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    public AuthorizeAttribute()
    {
        MinimalRoleLevel = Roles.User;
    }
    
    public AuthorizeAttribute(Roles role)
    {
        MinimalRoleLevel = role;
    }
    
    public Roles MinimalRoleLevel;

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var isExcluded = context.ActionDescriptor.EndpointMetadata
            .Any(em => em.GetType() == typeof(UnAuthorize));
        
        if(isExcluded) 
            return;
        
        var requestContextDataProvider =
            context.HttpContext.RequestServices.GetRequiredService<RequestContextDataProvider>();
        
        var cookie = context.HttpContext.Request.Cookies["Token"];
        
        var dbContext = context.HttpContext.RequestServices.GetRequiredService<PostgresContext>();

        if (string.IsNullOrEmpty(cookie))
            throw new ForbiddenException();

        var token = new Guid(cookie);
        var tokenObject = await dbContext.Tokens.Include(z => z.User).FirstOrDefaultAsync(x => x.TokenString == token);

        if(tokenObject == null)
            throw new ForbiddenException();
        
        var user = tokenObject.User;

        if (user.Role > MinimalRoleLevel)
            throw new ForbiddenException();

        var identifyUser = new IdentifyUserDto()
        {
            UserId = user.Id,
            Role = user.Role,
            FullName = user.FullName,
            TokenExpirationDate = tokenObject.ExpirationDate,
            Username = user.Username,
            Token = token
        };
        requestContextDataProvider.User = identifyUser;
    }
}