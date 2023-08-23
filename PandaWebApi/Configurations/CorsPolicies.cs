namespace PandaWebApi.Configurations;

public static class CorsPolicies
{
    public static void AddCorsToAllowAll(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()));
    }

    public static void UseCorsToAllowAll(this WebApplication app)
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