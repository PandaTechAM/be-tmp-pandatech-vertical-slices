using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PandaWebApi.Extensions;

public static class HealthCheckRunnerExtension
{
    public static void EnsureHealthy(this WebApplication app)
    {
        var healthCheckService = app.Services.GetRequiredService<HealthCheckService>();
        var report = healthCheckService.CheckHealthAsync().Result;

        if (report.Status == HealthStatus.Unhealthy)
        {
            var unhealthyChecks = report.Entries
                .Where(e => e.Value.Status != HealthStatus.Healthy)
                .Select(e => $"{e.Key}: {e.Value.Status}")
                .ToList();

            var message = $"Unhealthy services detected: {string.Join(", ", unhealthyChecks)}";
            throw new InvalidOperationException(message);
        }
    }
}