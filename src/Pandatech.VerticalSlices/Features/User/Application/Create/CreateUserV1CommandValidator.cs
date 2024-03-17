using FluentValidation;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.SharedKernel.Helpers;

namespace Pandatech.VerticalSlices.Features.User.Application.Create;

public class CreateUserV1CommandValidator : AbstractValidator<CreateUserV1Command>
{
   public CreateUserV1CommandValidator()
   {
      RuleFor(x => x.FullName).NotEmpty();

      RuleFor(x => x.Username)
         .NotEmpty();

      RuleFor(x => x.UserRole).IsInEnum();
      RuleFor(x => x.UserRole)
         .NotEqual(UserRole.SuperAdmin)
         .WithMessage("SuperAdmin cannot be created");
      RuleFor(x => x.Password)
         .Must(password => password.ValidatePassword())
         .WithMessage(PasswordHelper.WrongPasswordMessage);
   }
   //

   // private static async Task<bool> IsUniqueUsername(string username, PostgresContext dbContext,
   //    CancellationToken cancellationToken)
   // {
   //    return !await dbContext.Users.AnyAsync(u => u.Username == username, cancellationToken);
   // }
}
