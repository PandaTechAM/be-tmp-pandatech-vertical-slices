using Microsoft.AspNetCore.Mvc.Filters;
using ResponseCrafter.StandardHttpExceptions;

namespace PandaWebApi.Attributes;

public class ValidatorModelFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid)
        {
            return;
        }

        var errorDetails = new Dictionary<string, string>();
        foreach (var entry in context.ModelState)
        {
            if (!entry.Value.Errors.Any()) continue;

            var errorMessage = string.Join("; ", entry.Value.Errors.Select(e => e.ErrorMessage));
            errorDetails.Add(entry.Key, errorMessage);
        }

        throw new BadRequestException(errorDetails);
    }
}