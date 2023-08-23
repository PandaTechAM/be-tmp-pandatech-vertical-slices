using BaseConverter;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace PandaWebApi.Configurations;

public static class SwaggerConfiguration
{
    public static void AddSwaggerGen(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(config =>
        {
            // Add string input support into int64 field
            config.ParameterFilter<PandaParameterBaseConverter>();

            // Add the token authentication option
            config.AddSecurityDefinition("token", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Name = "token",
                Description = "Token authentication using the bearer scheme"
            });

            // Require the token to be passed as a header for API calls
            config.AddSecurityRequirement(new OpenApiSecurityRequirement
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