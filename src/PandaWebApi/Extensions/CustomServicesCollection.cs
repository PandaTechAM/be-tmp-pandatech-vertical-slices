using Pandatech.Crypto;
using PandaVaultClient;
using PandaWebApi.Helpers;

namespace PandaWebApi.Extensions;

public static class CustomServicesCollection
{
    public static WebApplicationBuilder RegisterAllCustomServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<DatabaseHelper>();
        builder.Services.AddPandatechCryptoAes256(o => o.Key = builder.Configuration["Security:AESKey"]!);
        builder.Services.AddPandatechCryptoArgon2Id();
        return builder;
    }
}