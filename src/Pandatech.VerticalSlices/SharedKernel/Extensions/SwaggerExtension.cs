using BaseConverter.Attributes;
using BaseConverter.Extensions;
using BaseConverter.Filters;
using Microsoft.OpenApi.Models;
using Pandatech.VerticalSlices.SharedKernel.Configurations.Options;
using Pandatech.VerticalSlices.SharedKernel.Helpers;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Pandatech.VerticalSlices.SharedKernel.Extensions;

public static class SwaggerExtension
{
   private static SwaggerOptions GetSwaggerOptions(IConfiguration configuration)
   {
      var swaggerOptions = configuration.GetSection("Swagger").Get<SwaggerOptions>();

      return swaggerOptions!;
   }

   public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
   {
      builder.Services.AddSwaggerGen(options =>
      {
         var swaggerOptions = GetSwaggerOptions(builder.Configuration);

         foreach (var version in swaggerOptions.Versions)
         {
            options.SwaggerDoc(version.Key,
               new OpenApiInfo
               {
                  Version = version.Key,
                  Title = version.Value.Title,
                  Description = version.Value.Description,
                  Contact = new OpenApiContact
                  {
                     Name = "PandaTech LLC", Email = "info@pandatech.it", Url = new Uri("https://pandatech.it")
                  }
               });
         }

         // Add string input support into int64 field
         options.AddBaseConverterFilters();
         options.SchemaFilter<EnumSwaggerSchemaFilter>();

         // Add the custom token authentication option
         options.AddSecurityDefinitions();
      });

      return builder;
   }

   public static void UseSwagger(this WebApplication app, IConfiguration configuration)
   {
      if (app.Environment.IsProduction())
      {
         return;
      }

      var swaggerOptions = configuration.GetSection("Swagger").Get<SwaggerOptions>();

      app.UseSwagger();
      app.UseSwaggerUI(options =>
      {
         if (swaggerOptions != null)
         {
            foreach (var version in swaggerOptions.Versions)
            {
               options.SwaggerEndpoint($"/swagger/{version.Key}/swagger.json", version.Value.Title);
            }
         }

         options.RoutePrefix = "swagger";
         options.DocumentTitle = $"Swagger - {AppDomain.CurrentDomain.FriendlyName}";

         options.InjectStylesheet("/assets/css/panda-style.css");
         options.InjectJavascript("/assets/js/docs.js");
         options.DocExpansion(DocExpansion.None);
      });

      if (swaggerOptions == null)
      {
         return;
      }

      foreach (var version in swaggerOptions.Versions.Where(version => version.Value.Separate))
      {
         app.UseSwaggerUI(options =>
         {
            options.SwaggerEndpoint($"/swagger/{version.Key}/swagger.json", version.Value.Title);

            options.RoutePrefix = $"swagger/{version.Value.RoutePrefix}";

            options.InjectStylesheet("/assets/css/panda-style.css");
            options.InjectJavascript("/assets/js/docs.js");
            options.DocExpansion(DocExpansion.None);
         });
      }
   }

   private static void AddSecurityDefinitions(this SwaggerGenOptions options)
   {
      var securityHeaders = new List<(string Name, string Description)>
      {
         ("Client-Type", "Client type, e.g., '2'"),
         ("Authorization", "Access token for the API"),
         ("Refresh-Token", "Refresh token for the access token refresh"),
         ("Accept-Language", "Language, e.g., 'en-US'")
      };

      foreach (var (name, description) in securityHeaders)
      {
         options.AddSecurityDefinition(name,
            new OpenApiSecurityScheme
            {
               Type = SecuritySchemeType.ApiKey,
               In = ParameterLocation.Header,
               Name = name,
               Description = description
            });

         options.AddSecurityRequirement(new OpenApiSecurityRequirement
         {
            {
               new OpenApiSecurityScheme
               {
                  Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = name },
                  In = ParameterLocation.Header
               },
               new List<string>()
            }
         });
      }
   }
}
