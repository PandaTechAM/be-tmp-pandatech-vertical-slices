using FluentValidation;
using Pandatech.VerticalSlices.SharedKernel.Helpers;
using RegexBox;

namespace Pandatech.VerticalSlices.Features.Auth.Application.RefreshToken;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
   public RefreshTokenCommandValidator()
   {
      RuleFor(x => x.RefreshTokenSignature)
         .NotEmpty()
         .Must(PandaValidator.IsGuid)
         .WithMessage(ErrorMessages.InvalidTokenFormat);
   }
}
