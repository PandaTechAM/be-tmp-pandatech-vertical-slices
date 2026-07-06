using FluentValidation;
using Pandatech.VerticalSlices.SharedKernel.Helpers;
using SharedKernel.Helpers;

namespace Pandatech.VerticalSlices.Features.Auth.Application.RefreshToken;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshTokenSignature)
            .NotEmpty()
            .Must(ValidationHelper.IsGuid)
            .WithMessage(ErrorMessages.InvalidTokenFormat);
    }
}
