using BaseConverter;
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
      List<long> ids = [];
      ids.AddRange(request.Ids.Select(PandaBaseConverter.Base36ToBase10NotNull));


      var users = await postgresContext.Users
         .Where(x => ids.Contains(x.Id))
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
