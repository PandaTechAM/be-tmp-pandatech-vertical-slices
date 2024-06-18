using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pandatech.Crypto;
using Pandatech.VerticalSlices.Context;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.Features.Auth.Application.RevokeAllTokensExceptCurrentSession;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;
using ResponseCrafter.HttpExceptions;

namespace Pandatech.VerticalSlices.Features.Auth.Application.UpdateOwnPassword;

public class UpdateOwnPasswordCommandHandler(
   IRequestContext requestContext,
   PostgresContext postgresContext,
   Argon2Id argon2Id,
   ISender sender) : ICommandHandler<UpdateOwnPasswordCommand>
{
   public async Task Handle(UpdateOwnPasswordCommand request, CancellationToken cancellationToken)
   {
      var user = await postgresContext
         .Users
         .FirstOrDefaultAsync(x => x.Id == requestContext.Identity.UserId && x.Role != UserRole.SuperAdmin,
            cancellationToken);

      if (user is null)
      {
         throw new InternalServerErrorException("User not found");
      }

      user.PasswordHash = argon2Id.HashPassword(request.NewPassword);

      user.MarkAsUpdated(requestContext.Identity.UserId);

      await postgresContext.SaveChangesAsync(cancellationToken);

      BackgroundJob.Enqueue<ISender>(x => x.Send(new RevokeAllTokensExceptCurrentCommand(), cancellationToken));
   }
}
