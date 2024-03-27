using Pandatech.VerticalSlices.SharedKernel.Extensions;
using Pandatech.VerticalSlices.SharedKernel.Helpers;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;
using StackExchange.Redis.Extensions.Protobuf;

namespace Pandatech.VerticalSlices.Infrastructure.Extensions;

internal static class RedisExtension
{
   internal static WebApplicationBuilder AddRedisCache(this WebApplicationBuilder builder)
   {
      var redisConnectionString = builder.Configuration.GetConnectionString(ConfigurationPaths.RedisUrl);
      builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString!));

      var redisConfiguration = new RedisConfiguration { ConnectionString = redisConnectionString };

      if (builder.Environment.IsLocalOrDevelopmentOrQa())
      {
         builder.Services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfiguration);
      }
      else
      {
         builder.Services.AddStackExchangeRedisExtensions<ProtobufSerializer>(redisConfiguration);
      }


      //builder.Services.AddSingleton<ILoggedUserCacheService, LoggedUserCacheService>();

      return builder;
   }
}
