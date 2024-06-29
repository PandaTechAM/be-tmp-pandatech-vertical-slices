using FluentValidation;

namespace Pandatech.VerticalSlices.Features.UserConfig.Application.CreateOrUpdate;

public class CreateOrUpdateUserConfigCommandValidator : AbstractValidator<CreateOrUpdateUserConfigCommand>
{
   public CreateOrUpdateUserConfigCommandValidator()
   {
      RuleFor(x => x.Configs)
         .NotEmpty();
   }
}
