using FluentValidation;

namespace Pandatech.VerticalSlices.Features.UserConfig.Application.Get;

public class GetUserConfigsQueryValidator : AbstractValidator<GetUserConfigsQuery>
{
   public GetUserConfigsQueryValidator()
   {
      RuleFor(x => x.Keys)
         .NotEmpty();

      RuleForEach(x => x.Keys)
         .MaximumLength(256);
   }
}
