using BaseConverter;
using Pandatech.Crypto;
using PandaTech.IEnumerableFilters.Extensions;
using PandaWebApi.DTOs;
using PandaWebApi.DTOs.User;
using PandaWebApi.Helpers;
using PandaWebApi.Services.Implementations;
using PandaWebApi.Services.Interfaces;

namespace PandaWebApi.Extensions;

public static class RegisterServicesExtension
{
    public static WebApplicationBuilder RegisterAllServices(this WebApplicationBuilder builder)
    {
        builder.AddServices()
            .AddPandaStandardServices();

        builder.Services.AddScoped<ITestJob, TestJob>(); //todo this is for illustration and testing purposes only.

        builder.Services.AddHttpContextAccessor();
        return builder;
    }

    private static WebApplicationBuilder AddPandaStandardServices(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsLocal())
        {
            builder.Services.AddSingleton<DatabaseHelper>();
        }

        builder.ConfigureBaseConverter(builder.Configuration["Security:Base36Chars"]!);
        builder.ConfigureEncryptedConverter(builder.Configuration["Security:AesKey"]!);
        builder.Services.AddPandatechCryptoAes256(o => o.Key = builder.Configuration["Security:AesKey"]!);
        builder.Services.AddPandatechCryptoArgon2Id();

        return builder;
    }

    private static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<ContextUser>();
        builder.Services.AddScoped<ITokenService, TokenService>();

        return builder;
    }
}