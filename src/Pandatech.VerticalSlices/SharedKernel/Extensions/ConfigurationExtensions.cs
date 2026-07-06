namespace Pandatech.VerticalSlices.SharedKernel.Extensions;

public static class ConfigurationExtensions
{
    private const string AesKeyConfigurationPath = "Security:AESKey";
    private const string RepositoryNameConfigurationPath = "RepositoryName";
    private const string RedisConfigurationPath = "Redis";
    private const string RabbitMqConfigurationPath = "RabbitMq";
    private const string PostgresConfigurationPath = "Postgres";
    private const string HangfireUserConfigurationPath = "Security:Hangfire:Username";
    private const string HangfirePasswordConfigurationPath = "Security:Hangfire:Password";
    private const string SuperUsernameConfigurationPath = "Security:SuperUser:Username";
    private const string SuperUserPasswordConfigurationPath = "Security:SuperUser:Password";
    private const string CookieDomainConfigurationPath = "Security:CookieDomain";

    extension(IConfiguration configuration)
    {
        public string GetAesKey()
        {
            return configuration[AesKeyConfigurationPath]!;
        }

        public string GetRepositoryName()
        {
            return configuration[RepositoryNameConfigurationPath]!;
        }

        public string GetRedisUrl()
        {
            return configuration.GetConnectionString(RedisConfigurationPath)!;
        }

        public string GetRabbitMqUrl()
        {
            return configuration.GetConnectionString(RabbitMqConfigurationPath)!;
        }

        public string GetPostgresUrl()
        {
            return configuration.GetConnectionString(PostgresConfigurationPath)!;
        }

        public string GetHangfireUsername()
        {
            return configuration[HangfireUserConfigurationPath]!;
        }

        public string GetHangfirePassword()
        {
            return configuration[HangfirePasswordConfigurationPath]!;
        }

        public string GetSuperUsername()
        {
            return configuration[SuperUsernameConfigurationPath]!;
        }

        public string GetSuperuserPassword()
        {
            return configuration[SuperUserPasswordConfigurationPath]!;
        }

        public string GetCookieDomain()
        {
            return configuration[CookieDomainConfigurationPath]!;
        }
    }
}
