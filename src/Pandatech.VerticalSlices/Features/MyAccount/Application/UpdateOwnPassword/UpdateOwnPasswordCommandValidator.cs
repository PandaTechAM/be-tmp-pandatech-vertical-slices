using FluentValidation;
using Pandatech.VerticalSlices.SharedKernel.Helpers;

namespace Pandatech.VerticalSlices.Features.MyAccount.Application.UpdateOwnPassword;

public class UpdateOwnPasswordCommandValidator : AbstractValidator<UpdateOwnPasswordCommand>
{
   public UpdateOwnPasswordCommandValidator()
   {
      RuleFor(x => x.OldPassword).NotEmpty()
         .Must(PasswordHelper.ValidatePassword)
         .WithMessage(PasswordHelper.WrongPasswordMessage);
      RuleFor(x => x.NewPassword).NotEmpty()
         .Must(PasswordHelper.ValidatePassword)
         .WithMessage(PasswordHelper.WrongPasswordMessage)
         .NotEqual(x => x.OldPassword)
         .WithMessage(ErrorMessages.NewPasswordMustBeDifferentFromOldPassword);
   }
}
