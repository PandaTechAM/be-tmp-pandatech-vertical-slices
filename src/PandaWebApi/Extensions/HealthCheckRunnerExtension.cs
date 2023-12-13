using System.Runtime.InteropServices;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ResponseCrafter.StandardHttpExceptions;

namespace PandaWebApi.Extensions;

public static class HealthCheckRunnerExtension
{
    public static WebApplication EnsureHealthy(this WebApplication app)
    {
        var healthCheckService = app.Services.GetRequiredService<HealthCheckService>();
        var report = healthCheckService.CheckHealthAsync().Result;

        if (report.Status != HealthStatus.Unhealthy) return app;
        var unhealthyChecks = report.Entries
            .Where(e => e.Value.Status != HealthStatus.Healthy)
            .Select(e => $"{e.Key}: {e.Value.Status}")
            .ToList();

        var message = $"Unhealthy services detected: {string.Join(", ", unhealthyChecks)}";
        throw new ServiceUnavailableException(message);

    }
}