using FluentValidation;
using Pandatech.VerticalSlices.SharedKernel.Helpers;

namespace Pandatech.VerticalSlices.Features.Auth.Application.UpdatePasswordForced;

public class UpdatePasswordForcedCommandValidator : AbstractValidator<UpdatePasswordForcedCommand>
{
   public UpdatePasswordForcedCommandValidator()
   {
      RuleFor(x => x.NewPassword).NotEmpty()
         .Must(PasswordHelper.ValidatePassword)
         .WithMessage(PasswordHelper.WrongPasswordMessage);
   }
}
