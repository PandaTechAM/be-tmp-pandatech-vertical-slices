using FluentValidation;
using Pandatech.VerticalSlices.SharedKernel.Helpers;

namespace Pandatech.VerticalSlices.Features.User.Application.UpdatePassword;

public class UpdateUserPasswordV1CommandValidator : AbstractValidator<UpdateUserPasswordV1Command>
{
   public UpdateUserPasswordV1CommandValidator()
   {
      RuleFor(x => x.Id).NotEmpty();

      RuleFor(x => x.NewPassword).NotEmpty()
         .Must(PasswordHelper.ValidatePassword)
         .WithMessage(PasswordHelper.WrongPasswordMessage);
   }
}
