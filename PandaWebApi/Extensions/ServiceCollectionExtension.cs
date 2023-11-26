﻿using PandaVaultClient;
using PandaWebApi.Helpers;

namespace PandaWebApi.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection RegisterAllCustomServices(this IServiceCollection services)
    {
        services.AddSingleton<DatabaseHelper>();
        services.RegisterPandaVault();
        return services;
    }
}