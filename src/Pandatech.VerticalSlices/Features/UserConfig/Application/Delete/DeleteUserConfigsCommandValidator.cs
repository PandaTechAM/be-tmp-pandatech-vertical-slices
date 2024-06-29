using FluentValidation;

namespace Pandatech.VerticalSlices.Features.UserConfig.Application.Delete;

public class DeleteUserConfigsCommandValidator : AbstractValidator<DeleteUserConfigsCommand>
{
   public DeleteUserConfigsCommandValidator()
   {
      RuleFor(x => x.Keys)
         .NotEmpty();

      RuleForEach(x => x.Keys)
         .MaximumLength(256);
   }
}
