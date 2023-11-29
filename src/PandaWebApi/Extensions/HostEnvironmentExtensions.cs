namespace PandaWebApi.Extensions;

public static class HostEnvironmentExtensions
{
    public static bool IsQa(this IHostEnvironment hostEnvironment)
    {
        if (hostEnvironment == null)
        {
            throw new ArgumentNullException(nameof(hostEnvironment));
        }

        return hostEnvironment.IsEnvironment("QA");
    }

    public static bool IsLocal(this IHostEnvironment hostEnvironment)
    {
        if (hostEnvironment == null)
        {
            throw new ArgumentNullException(nameof(hostEnvironment));
        }

        return hostEnvironment.IsEnvironment("Local");
    }
}
