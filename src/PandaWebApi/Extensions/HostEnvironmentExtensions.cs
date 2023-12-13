namespace PandaWebApi.Extensions;

public static class HostEnvironmentExtensions
{
    public static bool IsQa(this IHostEnvironment hostEnvironment)
    {
        ArgumentNullException.ThrowIfNull(hostEnvironment);

        return hostEnvironment.IsEnvironment("QA");
    }

    public static bool IsLocal(this IHostEnvironment hostEnvironment)
    {
        ArgumentNullException.ThrowIfNull(hostEnvironment);

        return hostEnvironment.IsEnvironment("Local");
    }
}
