using FluentValidation;
using RegexBox;

namespace Pandatech.VerticalSlices.Features.Auth.Application.Authenticate;

public class AuthenticateV1QueryValidator : AbstractValidator<AuthenticateV1Query>
{
   public AuthenticateV1QueryValidator()
   {
      RuleFor(x => x.AccessTokenSignature).NotEmpty()
         .Must(PandaValidator.IsGuid)
         .WithMessage("Invalid token signature");
   }
}
