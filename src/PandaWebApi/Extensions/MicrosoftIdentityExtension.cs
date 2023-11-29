using Microsoft.AspNetCore.Identity;
using PandaWebApi.Contexts;
using PandaWebApi.Models;

namespace PandaWebApi.Extensions;

public static class MicrosoftIdentityExtension
{
    public static WebApplicationBuilder AddMicrosoftIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
        builder.Services.AddAuthorizationBuilder();
        builder.Services.AddIdentityCore<User>(o =>
            {
                o.User.RequireUniqueEmail = false;
                o.Password.RequiredLength = 8;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequireLowercase = true;
                o.Password.RequireDigit = true;
                o.Password.RequireUppercase = true;
            })
            .AddEntityFrameworkStores<PostgresContext>().AddApiEndpoints();
        return builder;
    }
}