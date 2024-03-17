using RegexBox;

namespace Pandatech.VerticalSlices.SharedKernel.Extensions;

public static class CorsExtensions
{
    public static WebApplicationBuilder AddCors(this WebApplicationBuilder builder)
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
                "The ORIGINS environment variable is empty or incorrectly formatted.");
        }

        foreach (var origin in originsArray)
        {
            if (!PandaValidator.IsUri(origin, true, false))
            {
                throw new InvalidOperationException(
                    $"The origin {origin} in the ORIGINS environment variable is not valid.");
            }
        }
    }
}
