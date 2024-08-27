namespace Pandatech.VerticalSlices.SharedKernel.Helpers;

public static class ConfigurationHelper
{
   private const string AesKeyConfigurationPath = "Security:AESKey";
   private const string RedisConfigurationPath = "Redis";
   private const string RabbitMqConfigurationPath = "RabbitMq";
   private const string PostgresConfigurationPath = "Postgres";
   private const string CorsOriginsConfigurationPath = "Security:AllowedCorsOrigins";
   private const string HangfireUserConfigurationPath = "Security:Hangfire:Username";
   private const string HangfirePasswordConfigurationPath = "Security:Hangfire:Password";
   private const string SuperUsernameConfigurationPath = "Security:SuperUser:Username";
   private const string SuperUserPasswordConfigurationPath = "Security:SuperUser:Password";
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
      return configuration[AesKeyConfigurationPath]!;
   }

   public static string GetRedisUrl(this IConfiguration configuration)
   {
      return configuration.GetConnectionString(RedisConfigurationPath)!;
   }

   public static string GetRabbitMqUrl(this IConfiguration configuration)
   {
      return configuration.GetConnectionString(RabbitMqConfigurationPath)!;
   }

   public static string GetPostgresUrl(this IConfiguration configuration)
   {
      return configuration.GetConnectionString(PostgresConfigurationPath)!;
   }

   public static string GetAllowedCorsOrigins(this IConfiguration configuration)
   {
      return configuration[CorsOriginsConfigurationPath]!;
   }

   public static string GetHangfireUsername(this IConfiguration configuration)
   {
      return configuration[HangfireUserConfigurationPath]!;
   }

   public static string GetHangfirePassword(this IConfiguration configuration)
   {
      return configuration[HangfirePasswordConfigurationPath]!;
   }

   public static string GetSuperUsername(this IConfiguration configuration)
   {
      return configuration[SuperUsernameConfigurationPath]!;
   }

   public static string GetSuperuserPassword(this IConfiguration configuration)
   {
      return configuration[SuperUserPasswordConfigurationPath]!;
   }
}