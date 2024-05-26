using Microsoft.EntityFrameworkCore;
using Pandatech.VerticalSlices.Infrastructure.Context;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;
using ResponseCrafter.HttpExceptions;

namespace Pandatech.VerticalSlices.Features.User.Application.Update;

public class UpdateUserV1CommandHandler(PostgresContext postgresContext, IRequestContext requestContext)
   : ICommandHandler<UpdateUserV1Command>
{
   public async Task Handle(UpdateUserV1Command request, CancellationToken cancellationToken)
   {
      var user = await postgresContext.Users.FindAsync([request.Id], cancellationToken);

      if (user is null)
      {
         throw new NotFoundException("User not found");
      }

      var username = request.Username.ToLower();

      if (user.Username != username)
      {
         var duplicateUser =
            await postgresContext.Users.AnyAsync(x => x.Username == request.Username, cancellationToken);

         if (duplicateUser)
         {
            throw new ConflictException("username_already_exists");
         }
      }

      user.Username = username;
      user.FullName = request.FullName;
      user.Role = request.Role;
      user.Comment = request.Comment;

//      user.MarkAsUpdated(requestContext.Identity.UserId);

      await postgresContext.SaveChangesAsync(cancellationToken);
   }
}
