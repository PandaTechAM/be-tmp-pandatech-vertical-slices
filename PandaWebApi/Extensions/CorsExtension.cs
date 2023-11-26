using RegexBox;

namespace PandaWebApi.Extensions;

public static class CorsExtension
{
    public static void AddCors(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        if (builder.Environment.IsProduction())
        {
            var allowedOrigins = configuration["CorsSettings:AllowedOrigins"];

            ValidateCorsOrigins(allowedOrigins!);

            builder.Services.AddCors(options => options.AddPolicy("AllowSpecific", p => p
                .WithOrigins(allowedOrigins!)
                .AllowAnyMethod()
                .AllowAnyHeader()));
        }

        else
        {
            builder.Services.AddCors(options => options.AddPolicy("AllowAll", p => p
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
        }
    }

    public static void UseCors(this WebApplication app)
    {
        app.UseCors(app.Environment.IsProduction() ? "AllowSpecific" : "AllowAll");
    }

    private static void ValidateCorsOrigins(string allowedOrigins)
    {
        var originsArray = allowedOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries);

        if (originsArray.Length == 0)
        {
            throw new InvalidOperationException(
                "The ORIGINS environment variable is empty or incorrectly formatted.");
        }

        foreach (var origin in originsArray)
        {
            if (PandaValidator.IsUri(origin, true, false))
            {
                throw new InvalidOperationException(
                    $"The origin {origin} in the ORIGINS environment variable is not valid.");
            }
        }
    }
}