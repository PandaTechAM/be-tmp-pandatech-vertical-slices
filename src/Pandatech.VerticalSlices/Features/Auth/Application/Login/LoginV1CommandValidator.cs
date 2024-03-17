using FluentValidation;
using Pandatech.VerticalSlices.SharedKernel.Helpers;

namespace Pandatech.VerticalSlices.Features.Auth.Application.Login;

public class LoginV1CommandValidator : AbstractValidator<LoginV1Command>
{
   public LoginV1CommandValidator()
   {
      RuleFor(x => x.Username).NotEmpty();
      RuleFor(x => x.Password)
         .Must(password => password.ValidatePassword())
         .WithMessage(PasswordHelper.WrongPasswordMessage);
   }
}
