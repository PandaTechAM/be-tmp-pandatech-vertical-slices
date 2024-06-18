using FluentValidation;
using RegexBox;

namespace Pandatech.VerticalSlices.Features.Auth.Application.RefreshToken;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
   public RefreshTokenCommandValidator()
   {
         RuleFor(x => x.RefreshTokenSignature).NotEmpty()
            .Must(PandaValidator.IsGuid).WithMessage("Invalid refresh token signature");
      }
}