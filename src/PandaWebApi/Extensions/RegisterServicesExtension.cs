using BaseConverter;
using Pandatech.Crypto;
using PandaTech.IEnumerableFilters.Extensions;
using PandaWebApi.Helpers;
using PandaWebApi.Services.Implementations;
using PandaWebApi.Services.Interfaces;

namespace PandaWebApi.Extensions;

public static class RegisterServicesExtension
{
    public static WebApplicationBuilder RegisterAllServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUserManagementService, UserManagementService>();
        builder.Services.AddScoped<RequestContextDataProvider>();
        builder.AddPandaStandardServices();
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
}