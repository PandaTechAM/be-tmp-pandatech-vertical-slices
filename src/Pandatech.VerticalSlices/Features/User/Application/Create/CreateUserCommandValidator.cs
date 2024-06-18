using FluentValidation;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.SharedKernel.Helpers;

namespace Pandatech.VerticalSlices.Features.User.Application.Create;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
   public CreateUserCommandValidator()
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
