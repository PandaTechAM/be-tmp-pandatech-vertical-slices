using FluentValidation;
using MediatR;
using ResponseCrafter.HttpExceptions;

namespace Pandatech.VerticalSlices.SharedKernel.Behaviors;

public class ValidationBehaviorWithoutResponse<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
   : IPipelineBehavior<TRequest, TResponse>
   where TRequest : IRequest
{
   public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
      CancellationToken cancellationToken)
   {
      if (!validators.Any())
      {
         return await next();
      }

      var context = new ValidationContext<TRequest>(request);

      var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
      var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

      if (failures.Count == 0)
      {
         return await next();
      }

      var errors = failures.GroupBy(e => e.PropertyName, e => e.ErrorMessage)
         .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.First());

      throw new BadRequestException(errors);
   }
}