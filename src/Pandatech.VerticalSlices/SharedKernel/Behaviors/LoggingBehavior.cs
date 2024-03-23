using System.Diagnostics;
using System.Reflection;
using MediatR;

namespace Pandatech.VerticalSlices.SharedKernel.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<Mediator> logger, IHostEnvironment environment)
   : IPipelineBehavior<TRequest, TResponse>
   where TRequest : IRequest<TResponse>
{
   public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
      CancellationToken cancellationToken)
   {
      if (request is null)
      {
         throw new ArgumentNullException(nameof(request));
      }

      if (logger.IsEnabled(LogLevel.Information))
      {
         logger.LogInformation("Handling {RequestName}", typeof(TRequest).Name);

         // Reflection! Could be a performance concern and also expose sensitive data
         if (!environment.IsProduction())
         {
            Type myType = request.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
            foreach (PropertyInfo prop in props)
            {
               object? propValue = prop?.GetValue(request, null);
               logger.LogInformation("Property {Property} : {@Value}", prop?.Name, propValue);
            }
         }
      }

      var sw = Stopwatch.StartNew();

      var response = await next();
      sw.Stop();

      logger.LogInformation("Handled {RequestName} with {Response} in {Ms} ms", typeof(TRequest).Name, response,
         sw.ElapsedMilliseconds);
      return response;
   }
}
