using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PandaWebApi.Helpers;

[SuppressMessage("ReSharper", "ConvertToPrimaryConstructor")] //todo how to make this global?
public class CustomHealthChecks : IHealthCheck
{
    private readonly string _baseUrl;
    private const string Endpoint = "/ping";
    private const string ExpectedResponse = "pong";

    public CustomHealthChecks(string baseUrl)
    {
        _baseUrl = baseUrl;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new())
    {
        try
        {
            var httpClient = new HttpClient();
            
            var response = await httpClient.GetAsync(_baseUrl + Endpoint, cancellationToken);

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return content == ExpectedResponse
                ? HealthCheckResult.Healthy()
                : HealthCheckResult.Degraded();
        }
        catch (Exception e)
        {
            return HealthCheckResult.Unhealthy(exception: e);
        }
    }
}