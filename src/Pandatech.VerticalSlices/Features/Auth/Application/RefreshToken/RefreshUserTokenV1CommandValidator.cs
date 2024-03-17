using FluentValidation;
using RegexBox;

namespace Pandatech.VerticalSlices.Features.Auth.Application.RefreshToken;

public class RefreshUserTokenV1CommandValidator : AbstractValidator<RefreshUserTokenV1Command>
{
   public RefreshUserTokenV1CommandValidator()
   {
      RuleFor(x => x.RefreshTokenSignature).NotEmpty()
         .Must(PandaValidator.IsGuid).WithMessage("Invalid refresh token signature");
   }
}
