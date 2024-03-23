using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.SharedKernel.Helpers;

namespace Pandatech.VerticalSlices.Features.User.Application.Create;

public class CreateUserV1CommandValidator : AbstractValidator<CreateUserV1Command>
{
   public CreateUserV1CommandValidator()
   {
      RuleFor(x => x.FullName).NotEmpty();

      RuleFor(x => x.Username).NotEmpty();

      RuleFor(x => x.UserRole).IsInEnum();
      RuleFor(x => x.UserRole)
         .NotEqual(UserRole.SuperAdmin)
         .WithMessage("not_supported_role");
      RuleFor(x => x.Password)
         .Must(password => password.ValidatePassword())
         .WithMessage(PasswordHelper.WrongPasswordMessage);
   }
}
