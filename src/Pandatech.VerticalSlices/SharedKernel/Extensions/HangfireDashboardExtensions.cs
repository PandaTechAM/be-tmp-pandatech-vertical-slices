using Hangfire;
using HangfireBasicAuthenticationFilter;

namespace Pandatech.VerticalSlices.SharedKernel.Extensions;

public static class HangfireDashboardExtensions
{
  public static WebApplication UseHangfireServer(this WebApplication app)
  {
    var user = app.Configuration["Security:Hangfire:Username"];
    var pass = app.Configuration["Security:Hangfire:Password"];

    app.UseHangfireDashboard("/hangfire",
      new DashboardOptions
      {
        DashboardTitle = "JobMaster Dashboard",
        Authorization = new[] { new HangfireCustomBasicAuthenticationFilter { User = user, Pass = pass } }
      });
    app.MapHangfireDashboard();

    return app;
  }
}
