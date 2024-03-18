using FluentValidation;

namespace Pandatech.VerticalSlices.Features.User.Application.GetById;

public class GetUserByIdV1QueryValidator : AbstractValidator<GetUserByIdV1Query>
{
   public GetUserByIdV1QueryValidator()
   {
      RuleFor(x => x.Id).NotEmpty();
   }
}
