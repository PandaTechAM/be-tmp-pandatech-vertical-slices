using Microsoft.EntityFrameworkCore;
using Pandatech.VerticalSlices.Context;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.SharedKernel.Helpers;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;
using ResponseCrafter.HttpExceptions;

namespace Pandatech.VerticalSlices.Features.User.Application.Update;

public class UpdateUserCommandHandler(PostgresContext postgresContext, IRequestContext requestContext)
   : ICommandHandler<UpdateUserCommand>
{
   public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
   {
      var user = await postgresContext
         .Users
         .FirstOrDefaultAsync(u => u.Id == request.Id && u.Role != UserRole.SuperAdmin, cancellationToken);

      NotFoundException.ThrowIfNull(user);


      var username = request.Username.ToLower();

      if (user.Username != username)
      {
         var duplicateUser =
            await postgresContext
               .Users
               .AnyAsync(x => x.Username == request.Username, cancellationToken);

         ConflictException.ThrowIf(duplicateUser, ErrorMessages.DuplicateUsername);

      }

      user.Username = username;
      user.FullName = request.FullName;
      user.Role = request.Role;
      user.Comment = request.Comment;
      user.MarkAsUpdated(requestContext.Identity.UserId);

      await postgresContext.SaveChangesAsync(cancellationToken);
   }
}
