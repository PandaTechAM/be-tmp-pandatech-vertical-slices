using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PandaWebApi.Configurations.Health;

public class UserManagementHealthCheck : IHealthCheck
{
    private readonly IHttpClientFactory _httpClientFactory;

    public UserManagementHealthCheck(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(
                Environment.GetEnvironmentVariable("USER_MANAGEMENT_ADDRESS") + "/ping",
                cancellationToken
            );

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return content == "pong"
                ? HealthCheckResult.Healthy()
                : HealthCheckResult.Unhealthy();
        }
        catch (Exception e)
        {
            return HealthCheckResult.Unhealthy(exception: e);
        }
    }
}