using PandaWebApi.Helpers;

namespace PandaWebApi.Extensions;

public static class CorsExtension
{
    public static void AddCors(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsProduction())
        {
            var allowedOrigins = Environment.GetEnvironmentVariable("CORS_ALLOWED_ORIGINS");

            var originsArray = allowedOrigins!.Split(',', StringSplitOptions.RemoveEmptyEntries);

            if (originsArray.Length == 0)
            {
                throw new InvalidOperationException(
                    "The ORIGINS environment variable is empty or incorrectly formatted.");
            }

            foreach (var origin in originsArray)
            {
                if (!RegExHelper.IsValidSecureUri(origin, true))
                {
                    throw new InvalidOperationException(
                        $"The origin {origin} in the ORIGINS environment variable is not valid.");
                }
            }

            builder.Services.AddCors(options => options.AddPolicy("AllowSpecific", p => p.WithOrigins(allowedOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader()));
        }

        else
        {
            builder.Services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
        }
    }

    public static void UseCors(this WebApplication app)
    {
        if (app.Environment.IsProduction())
        {
            app.UseCors("AllowSpecific");
        }
        else
        {
            app.UseCors(
                policyBuilder =>
                    policyBuilder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
            );
        }
    }
}