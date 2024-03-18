using FluentValidation;
using Pandatech.VerticalSlices.Domain.Enums;

namespace Pandatech.VerticalSlices.Features.User.Application.Update;

public class UpdateUserV1CommandValidator : AbstractValidator<UpdateUserV1Command>
{
   public UpdateUserV1CommandValidator()
   {
      RuleFor(x => x.Id).NotEmpty();
      RuleFor(x => x.Username).NotEmpty();
      RuleFor(x => x.FullName).NotEmpty();
      RuleFor(x => x.Role).NotEmpty().IsInEnum()
         .NotEqual(UserRole.SuperAdmin).WithMessage("SuperAdmin role is not allowed");
   }
}
