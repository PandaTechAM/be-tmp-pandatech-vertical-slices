using System.Diagnostics;
using System.Globalization;
using Newtonsoft.Json;

namespace Pandatech.VerticalSlices.SharedKernel.Extensions;

public static class StartupLogger
{
   private static readonly Stopwatch _stopwatch = new();

   public static void LogStartAttempt(WebApplicationBuilder builder)
   {
      _stopwatch.Start();
      var now = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
      Console.WriteLine(JsonConvert.SerializeObject(new
      {
         Timestamp = now,
         Event = "ApplicationStartAttempt",
         Application = builder.Environment.ApplicationName,
         Environment = builder.Environment.EnvironmentName
      }));
   }

   public static void LogStartSuccess()
   {
      _stopwatch.Stop();
      var now = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
      var initializationTime = Math.Round(_stopwatch.Elapsed.TotalMilliseconds / 1000, 2);
      Console.WriteLine(JsonConvert.SerializeObject(new
      {
         Timestamp = now, Event = "ApplicationStartSuccess", InitializationTime = $"{initializationTime} seconds"
      }));
   }
}
