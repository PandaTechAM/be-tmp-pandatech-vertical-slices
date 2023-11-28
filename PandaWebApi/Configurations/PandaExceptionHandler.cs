using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace PandaWebApi.Configurations;

public class PandaExceptionHandler : IExceptionHandler
{
    private readonly ILogger<PandaExceptionHandler> _logger;
    private readonly string _aspEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;

    public PandaExceptionHandler(ILogger<PandaExceptionHandler> logger)
    {
        _logger = logger;
    }


    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, exception.Message);

        
        if (_aspEnvironment is "Staging" or "Production")
        {
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Type = exception.GetType().Name,
                Title = "Internal Server Error",
                Detail = "Please try again later and/or contact IT support.",
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
            }, cancellationToken: cancellationToken);
        }
        else
        {
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Type = exception.GetType().Name,
                Title = "error.password",
                Detail = exception.Message,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
            }, cancellationToken: cancellationToken);
        }
        return true;
    }
}