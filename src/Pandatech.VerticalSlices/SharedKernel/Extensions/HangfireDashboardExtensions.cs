using Hangfire;
using HangfireBasicAuthenticationFilter;
using Pandatech.VerticalSlices.SharedKernel.Helpers;

namespace Pandatech.VerticalSlices.SharedKernel.Extensions;

public static class HangfireDashboardExtensions
{
   public static WebApplication UseHangfireServer(this WebApplication app)
   {
      var user = app.Configuration.GetHangfireUsername();
      var pass = app.Configuration.GetHangfirePassword();

      app.UseHangfireDashboard("/hangfire",
         new DashboardOptions
         {
            DashboardTitle = "JobMaster Dashboard",
            Authorization =
            [
               new HangfireCustomBasicAuthenticationFilter
               {
                  User = user,
                  Pass = pass
               }
            ]
         });
      app.MapHangfireDashboard();

      return app;
   }
}