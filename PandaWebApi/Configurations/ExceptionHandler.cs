using PandaTech.ServiceResponse;

namespace PandaWebApi.Configurations;

public static class ExceptionHandler
{
    public static void AddExceptionHandler(this WebApplicationBuilder builder)
    {
        if (!builder.Environment.IsProduction())
        {
            builder.Services.AddTransient<IExceptionHandler, DebugExceptionHandler>();
        }
        else
        {
            builder.Services.AddTransient<IExceptionHandler, PublicExceptionHandler>();
        }
    }
}