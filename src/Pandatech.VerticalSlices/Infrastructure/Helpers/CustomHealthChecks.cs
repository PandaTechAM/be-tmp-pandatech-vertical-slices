using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Pandatech.VerticalSlices.Infrastructure.Helpers;

public class CustomHealthChecks(string baseUrl) : IHealthCheck
{
   private const string Endpoint = "/above-board/ping";
   private const string ExpectedResponse = "pong";

   public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
      CancellationToken cancellationToken = new())
   {
      try
      {
         var httpClient = new HttpClient();

         var response = await httpClient.GetAsync(baseUrl + Endpoint, cancellationToken);

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
