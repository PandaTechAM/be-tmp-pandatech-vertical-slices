namespace WebApiPostgres.Configurations;

public abstract class CorsPolicies
{
    public static void AddCorsToAllowAll(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()));
    }

    public static void UseCorsToAllowAll(WebApplication app)
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