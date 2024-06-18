using Microsoft.EntityFrameworkCore;
using Pandatech.Crypto;
using Pandatech.VerticalSlices.Context;
using Pandatech.VerticalSlices.SharedKernel.Helpers;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;
using ResponseCrafter.HttpExceptions;

namespace Pandatech.VerticalSlices.Features.User.Application.Create;

public class CreateUserCommandHandler(PostgresContext dbContext, Argon2Id argon, IRequestContext requestContext)
   : ICommandHandler<CreateUserCommand>
{
   public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
   {
      var isDuplicateUsername = await dbContext.Users
         .AnyAsync(u => u.Username.ToLower().Equals(request.Username.ToLower()),
            cancellationToken);

      if (isDuplicateUsername)
      {
         throw new BadRequestException(ErrorMessages.DuplicateUsername);
      }

      var passwordHash = argon.HashPassword(request.Password);
      var user = new Domain.Entities.User
      {
         Username = request.Username.ToLower(),
         FullName = request.FullName,
         PasswordHash = passwordHash,
         Role = request.UserRole,
         Comment = request.Comment,
         CreatedByUserId = requestContext.Identity.UserId
      };

      await dbContext.Users.AddAsync(user, cancellationToken);
      await dbContext.SaveChangesAsync(cancellationToken);
   }
}
