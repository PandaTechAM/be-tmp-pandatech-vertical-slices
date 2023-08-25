using BaseConverter;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace PandaWebApi.Configurations;

public static class SwaggerConfiguration
{
    public static void AddSwaggerGen(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = $"PandaTech API - {AppDomain.CurrentDomain.FriendlyName}",
                Description =
                    "PandaTech is an enterprise software engineering company dedicated to providing top-tier fintech solutions and more. Leveraging cutting-edge technology and industry expertise, we strive to deliver excellence and innovation in every project.",
                Contact = new OpenApiContact
                {
                    Name = "PandaTech",
                    Email = "info@pandatech.it",
                    Url = new Uri("https://www.pandatech.it"),
                }
            });

            //This option is created because due to some bug /health endpoint is not working in .NET 7. It's included in Microsoft planning.
            options.DocumentFilter<HealthChecksFilter>();

            // Add string input support into int64 field
            options.ParameterFilter<PandaParameterBaseConverter>();

            // Add the token authentication option
            options.AddSecurityDefinition("token", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Name = "token",
                Description = "Token authentication using the bearer scheme"
            });

            // Require the token to be passed as a header for API calls
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "token"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    public static void UseSwagger(this WebApplication app)
    {
        if (app.Environment.IsProduction()) return;
        SwaggerBuilderExtensions.UseSwagger(app);
        app.UseSwaggerUI();
    }
}