using Pandatech.VerticalSlices.SharedKernel.Middlewares;

namespace Pandatech.VerticalSlices.SharedKernel.Extensions;

public static class LoggingExtensions
{
   public static WebApplication UseRequestResponseLogging(this WebApplication app)
   {
      if (app.Logger.IsEnabled(LogLevel.Information))
      {
         app.UseMiddleware<RequestResponseLoggingMiddleware>();
      }

      return app;
   }
}