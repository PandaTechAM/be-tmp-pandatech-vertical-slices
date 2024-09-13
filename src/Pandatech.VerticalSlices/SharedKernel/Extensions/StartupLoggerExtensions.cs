using System.Diagnostics;
using System.Globalization;
using Newtonsoft.Json;

namespace Pandatech.VerticalSlices.SharedKernel.Extensions;

public static class StartupLoggerExtensions
{
   private static readonly Stopwatch Stopwatch = new();

   public static WebApplicationBuilder LogStartAttempt(this WebApplicationBuilder builder)
   {
      Stopwatch.Start();
      var now = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
      Console.WriteLine(JsonConvert.SerializeObject(new
      {
         Timestamp = now,
         Event = "ApplicationStartAttempt",
         Application = builder.Environment.ApplicationName,
         Environment = builder.Environment.EnvironmentName
      }));
      return builder;
   }

   public static WebApplication LogStartSuccess(this WebApplication app)
   {
      Stopwatch.Stop();
      var now = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
      var initializationTime = Math.Round(Stopwatch.Elapsed.TotalMilliseconds / 1000, 2);
      Console.WriteLine(JsonConvert.SerializeObject(new
      {
         Timestamp = now,
         Event = "ApplicationStartSuccess",
         InitializationTime = $"{initializationTime} seconds"
      }));
      
      return app;
   }
}