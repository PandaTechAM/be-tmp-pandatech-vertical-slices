using PandaTech.ServiceResponse;

namespace WebApiPostgres.Configurations;

public abstract class ConfigureServiceResponse
{
    public static void AddExceptionHandling(WebApplicationBuilder builder)
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production")
        {
            builder.Services.AddTransient<IExceptionHandler, DebugExceptionHandler>();
        }
        else
        {
            builder.Services.AddTransient<IExceptionHandler, PublicExceptionHandler>();
        }
    }
}