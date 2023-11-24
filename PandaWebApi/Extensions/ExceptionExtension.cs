using PandaTech.ServiceResponse;

namespace PandaWebApi.Extensions;

public static class ExceptionExtension
{
    public static void AddExceptionHandler(this WebApplicationBuilder builder)
    {
        if (!builder.Environment.IsProduction() || !builder.Environment.IsStaging())
        {
            builder.Services.AddTransient<IExceptionHandler, DebugExceptionHandler>();
        }
        else
        {
            builder.Services.AddTransient<IExceptionHandler, PublicExceptionHandler>();
        }
    }
}