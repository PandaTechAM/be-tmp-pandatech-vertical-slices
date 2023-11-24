using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PandaWebApi.Extensions.Health;

[SuppressMessage("ReSharper", "ConvertToPrimaryConstructor")] //todo how to make this global?

public class CustomHealthChecks : IHealthCheck
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string Endpoint = "/ping";
    private const string ExpectedResponse = "pong";

    public CustomHealthChecks(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(Endpoint, cancellationToken);

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return content == ExpectedResponse
                ? HealthCheckResult.Healthy()
                : HealthCheckResult.Unhealthy();
        }
        catch (Exception e)
        {
            return HealthCheckResult.Unhealthy(exception: e);
        }
    }
}