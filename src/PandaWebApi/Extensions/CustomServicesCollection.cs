using BaseConverter;
using Pandatech.Crypto;
using PandaTech.IEnumerableFilters.Extensions;
using PandaWebApi.Helpers;

namespace PandaWebApi.Extensions;

public static class CustomServicesCollection
{
    public static WebApplicationBuilder RegisterAllServices(this WebApplicationBuilder builder)
    {
        builder.RegisterAllDefaultServices();
        return builder;
    }

    private static WebApplicationBuilder RegisterAllDefaultServices(this WebApplicationBuilder builder)
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