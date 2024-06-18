using FluentValidation;

namespace Pandatech.VerticalSlices.Features.User.Application.GetUser;

public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
   public GetUserQueryValidator()
   {
      RuleFor(x => x.Id).NotEmpty();
   }
}
