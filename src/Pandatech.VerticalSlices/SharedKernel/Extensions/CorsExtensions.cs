using Pandatech.VerticalSlices.SharedKernel.Helpers;
using RegexBox;

namespace Pandatech.VerticalSlices.SharedKernel.Extensions;

public static class CorsExtension
{
   public static WebApplicationBuilder AddCors(this WebApplicationBuilder builder)
   {
      var configuration = builder.Configuration;
      if (builder.Environment.IsProduction())
      {
         var allowedOrigins = configuration[ConfigurationPaths.CorsOrigins];

         ValidateCorsOrigins(allowedOrigins!);

         builder.Services.AddCors(options => options.AddPolicy("AllowSpecific", p => p
            .WithOrigins(allowedOrigins!)
            .AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader()));
      }
      else
      {
         builder.Services.AddCors(options => options.AddPolicy("AllowAll", p => p
            .SetIsOriginAllowed(_ => true)
            .AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader()));
      }

      return builder;
   }

   public static WebApplication UseCors(this WebApplication app)
   {
      app.UseCors(app.Environment.IsProduction() ? "AllowSpecific" : "AllowAll");
      return app;
   }

   private static void ValidateCorsOrigins(string allowedOrigins)
   {
      var originsArray = allowedOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries);

      if (originsArray.Length == 0)
      {
         throw new InvalidOperationException(
            "The Cors origins are empty or incorrectly formatted.");
      }

      foreach (var origin in originsArray)
      {
         if (!PandaValidator.IsUri(origin, true, false))
         {
            throw new InvalidOperationException(
               $"The origin {origin} is not valid URI.");
         }
      }
   }
}
