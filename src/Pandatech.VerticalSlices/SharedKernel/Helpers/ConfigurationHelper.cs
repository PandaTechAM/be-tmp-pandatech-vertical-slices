using System.Security.Cryptography;

namespace Pandatech.VerticalSlices.SharedKernel.Helpers;

public static class ConfigurationHelper
{
   private const string AesKey = "Security:AESKey";
   private const string RedisUrl = "Redis";
   private const string RabbitMqUrl = "RabbitMq";
   private const string PostgresUrl = "Postgres";
   private const string CorsOrigins = "Security:AllowedCorsOrigins";
   private const string HangfireUser = "Security:Hangfire:Username";
   private const string HangfirePassword = "Security:Hangfire:Password";
   private const string SuperUsername = "Security:SuperUser:Username";
   private const string SuperUserPassword = "Security:SuperUser:Password";
   private const string PersistentConfigurationPath = "PersistentStorage";
   private const string RepositoryNameConfigurationPath = "RepositoryName";

   public static string GetFileStoragePath(this IConfiguration configuration)
   {
      var persistencePath = configuration.GetPersistentPath();
      return $"{persistencePath}/files";
   }
   
   public static string GetRepositoryName(this IConfiguration configuration)
   {
      return configuration[RepositoryNameConfigurationPath]!;
   }
   
   public static string GetPersistentPath(this IConfiguration configuration)
   {
      return configuration.GetConnectionString(PersistentConfigurationPath)!;
   }

   public static string GetAesKey(this IConfiguration configuration)
   {
      return configuration[AesKey]!;
   }

   public static string GetRedisUrl(this IConfiguration configuration)
   {
      return configuration.GetConnectionString(RedisUrl)!;
   }

   public static string GetRabbitMqUrl(this IConfiguration configuration)
   {
      return configuration.GetConnectionString(RabbitMqUrl)!;
   }

   public static string GetPostgresUrl(this IConfiguration configuration)
   {
      return configuration.GetConnectionString(PostgresUrl)!;
   }

   public static string GetAllowedCorsOrigins(this IConfiguration configuration)
   {
      return configuration[CorsOrigins]!;
   }

   public static string GetHangfireUsername(this IConfiguration configuration)
   {
      return configuration[HangfireUser]!;
   }

   public static string GetHangfirePassword(this IConfiguration configuration)
   {
      return configuration[HangfirePassword]!;
   }

   public static string GetSuperUsername(this IConfiguration configuration)
   {
      return configuration[SuperUsername]!;
   }

   public static string GetSuperuserPassword(this IConfiguration configuration)
   {
      return configuration[SuperUserPassword]!;
   }
}