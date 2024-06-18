using Microsoft.EntityFrameworkCore;
using Pandatech.VerticalSlices.Context;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.User.Application.Delete;

public class DeleteUsersCommandHandler(PostgresContext postgresContext, IRequestContext requestContext)
   : ICommandHandler<DeleteUsersCommand>
{
   public async Task Handle(DeleteUsersCommand request, CancellationToken cancellationToken)
   {
      var users = await postgresContext.Users
         .Where(x => request.Ids.Contains(x.Id))
         .Where(x => x.Role != UserRole.SuperAdmin)
         .ToListAsync(cancellationToken);


      if (users.Count == 0)
      {
         return;
      }

      foreach (var user in users)
      {
         user.MarkAsDeleted(requestContext.Identity.UserId);
      }

      await postgresContext.SaveChangesAsync(cancellationToken);
   }
}
