using FluentValidation;

namespace Pandatech.VerticalSlices.Features.User.Application.UpdateStatus;

public class UpdateUserStatusV1CommandValidator : AbstractValidator<UpdateUserStatusV1Command>
{
   public UpdateUserStatusV1CommandValidator()
   {
      RuleFor(x => x.Id).NotEmpty();
      RuleFor(x => x.Status).IsInEnum();
   }
}
