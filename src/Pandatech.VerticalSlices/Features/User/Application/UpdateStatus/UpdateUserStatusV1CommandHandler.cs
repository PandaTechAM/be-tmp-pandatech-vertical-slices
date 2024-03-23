using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.Infrastructure.Context;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;
using ResponseCrafter.StandardHttpExceptions;

namespace Pandatech.VerticalSlices.Features.User.Application.UpdateStatus;

public class UpdateUserStatusV1CommandHandler(PostgresContext postgresContext)
   : ICommandHandler<UpdateUserStatusV1Command>
{
   public async Task Handle(UpdateUserStatusV1Command request, CancellationToken cancellationToken)
   {
      var user = await postgresContext.Users.FindAsync([request.Id], cancellationToken: cancellationToken);

      if (user is null || user.Role == UserRole.SuperAdmin)
      {
         throw new NotFoundException("User not found");
      }

      if (user.Status == request.Status)
      {
         throw new BadRequestException("status_already_set");
      }

      user.Status = request.Status;
      user.UpdatedAt = DateTime.UtcNow;

      await postgresContext.SaveChangesAsync(cancellationToken);
   }
}
