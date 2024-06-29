using Microsoft.EntityFrameworkCore;
using Pandatech.VerticalSlices.Context;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;
using ResponseCrafter.HttpExceptions;

namespace Pandatech.VerticalSlices.Features.User.Application.UpdateStatus;

public class UpdateUserStatusCommandHandler(PostgresContext postgresContext, IRequestContext requestContext)
   : ICommandHandler<UpdateUserStatusCommand>
{
   public async Task Handle(UpdateUserStatusCommand request, CancellationToken cancellationToken)
   {
      var user = await postgresContext
         .Users
         .FirstOrDefaultAsync(u => u.Id == request.Id && u.Role != UserRole.SuperAdmin, cancellationToken);

      NotFoundException.ThrowIfNull(user);

      if (user.Status == request.Status)
      {
         return;
      }

      user.Status = request.Status;
      user.MarkAsUpdated(requestContext.Identity.UserId);
      await postgresContext.SaveChangesAsync(cancellationToken);
   }
}
