namespace Pandatech.VerticalSlices.SharedKernel.Extensions;

public static class HostEnvironmentExtensions
{
   public static bool IsQa(this IHostEnvironment hostEnvironment)
   {
         ArgumentNullException.ThrowIfNull(hostEnvironment);

         return hostEnvironment.IsEnvironment("QA");
      }

   public static bool IsLocal(this IHostEnvironment hostEnvironment)
   {
         ArgumentNullException.ThrowIfNull(hostEnvironment);

         return hostEnvironment.IsEnvironment("Local");
      }

   public static bool IsLocalOrDevelopment(this IHostEnvironment hostEnvironment)
   {
         ArgumentNullException.ThrowIfNull(hostEnvironment);

         return hostEnvironment.IsLocal() || hostEnvironment.IsDevelopment();
      }

   public static bool IsLocalOrDevelopmentOrQa(this IHostEnvironment hostEnvironment)
   {
         ArgumentNullException.ThrowIfNull(hostEnvironment);

         return hostEnvironment.IsLocal() || hostEnvironment.IsDevelopment() || hostEnvironment.IsQa();
      }

   public static string GetShortEnvironmentName(this IHostEnvironment environment)
   {
         ArgumentNullException.ThrowIfNull(environment);

         if (environment.IsLocal())
         {
            return "local";
         }

         if (environment.IsDevelopment())
         {
            return "dev";
         }

         if (environment.IsQa())
         {
            return "qa";
         }

         if (environment.IsStaging())
         {
            return "staging";
         }

         return "";
      }
}