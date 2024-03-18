using BaseConverter;
using Microsoft.EntityFrameworkCore;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.Infrastructure.Contexts;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;
using ResponseCrafter.StandardHttpExceptions;

namespace Pandatech.VerticalSlices.Features.User.Application.Delete;

public class DeleteUsersV1CommandHandler(PostgresContext postgresContext) : ICommandHandler<DeleteUsersV1Command>
{
   public async Task Handle(DeleteUsersV1Command request, CancellationToken cancellationToken)
   {
      var users = await postgresContext.Users
         .Where(x => request.Ids.Contains(x.Id))
         .ToListAsync(cancellationToken);

      var notFoundIds = request.Ids
         .Except(users.Select(x => x.Id))
         .ToList();

      var alreadyDeletedIds = users
         .Where(x => x.Status == UserStatus.Deleted)
         .Select(x => x.Id)
         .ToList();

      var superAdminIds = users
         .Where(x => x.Role == UserRole.SuperAdmin)
         .Select(x => x.Id)
         .ToList();

      var errors = new Dictionary<string, string>();

      if (notFoundIds.Count != 0)
      {
         foreach (var base36Id in notFoundIds.Select(PandaBaseConverter.Base10ToBase36))
         {
            errors.Add("not_found", $"User with id {base36Id} not found");
         }
      }

      if (alreadyDeletedIds.Count != 0)
      {
         foreach (var base36Id in alreadyDeletedIds.Select(PandaBaseConverter.Base10ToBase36))
         {
            errors.Add("already_deleted", $"User with id {base36Id} already deleted");
         }
      }

      if (superAdminIds.Count != 0)
      {
         foreach (var base36Id in superAdminIds.Select(PandaBaseConverter.Base10ToBase36))
         {
            errors.Add("not_found", $"User with id {base36Id} not found"); // This is a security measure
         }
      }

      if (errors.Count != 0)
      {
         throw new BadRequestException(errors);
      }

      foreach (var user in users)
      {
         user.Status = UserStatus.Deleted;
         user.UpdatedAt = DateTime.UtcNow;
      }

      await postgresContext.SaveChangesAsync(cancellationToken);
   }
}
