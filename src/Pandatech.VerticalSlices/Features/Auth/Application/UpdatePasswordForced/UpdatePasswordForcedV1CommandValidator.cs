using FluentValidation;
using Pandatech.VerticalSlices.SharedKernel.Helpers;

namespace Pandatech.VerticalSlices.Features.Auth.Application.UpdatePasswordForced;

public class UpdatePasswordForcedV1CommandValidator : AbstractValidator<UpdatePasswordForcedV1Command>
{
   public UpdatePasswordForcedV1CommandValidator()
   {
      RuleFor(x => x.NewPassword).NotEmpty()
         .Must(PasswordHelper.ValidatePassword)
         .WithMessage(PasswordHelper.WrongPasswordMessage);
   }
}
