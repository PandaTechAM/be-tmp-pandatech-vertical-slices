using Pandatech.VerticalSlices.SharedKernel.Helpers;
using RegexBox;

namespace Pandatech.VerticalSlices.SharedKernel.Extensions;

public static class CorsExtension
{
   public static WebApplicationBuilder AddCors(this WebApplicationBuilder builder)
   {
      if (builder.Environment.IsProduction())
      {
         builder.Services.AddCors(options => options.AddPolicy("AllowSpecific",
            p => p
                 .WithOrigins(builder.Configuration
                                     .GetAllowedCorsOrigins()
                                     .SplitOrigins())
                 .AllowCredentials()
                 .AllowAnyMethod()
                 .AllowAnyHeader()));
      }
      else
      {
         builder.Services.AddCors(options => options.AddPolicy("AllowAll",
            p => p
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

   private static string[] SplitOrigins(this string input)
   {
      if (string.IsNullOrEmpty(input))
      {
         throw new ArgumentException("Cors Origins cannot be null or empty.");
      }

      var result = input.Split([';', ','], StringSplitOptions.RemoveEmptyEntries);

      for (var i = 0; i < result.Length; i++)
      {
         result[i] = result[i]
            .Trim();
      }

      return result;
   }
}