using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pandatech.Crypto;
using Pandatech.VerticalSlices.Features.Auth.Application.RevokeAllTokensExceptCurrentSession;
using Pandatech.VerticalSlices.Infrastructure.Context;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;
using ResponseCrafter.StandardHttpExceptions;

namespace Pandatech.VerticalSlices.Features.Auth.Application.UpdateOwnPassword;

public class UpdateOwnPasswordV1CommandHandler(
   IRequestContext requestContext,
   PostgresContext postgresContext,
   Argon2Id argon2Id,
   ISender sender) : ICommandHandler<UpdateOwnPasswordV1Command>
{
   public async Task Handle(UpdateOwnPasswordV1Command request, CancellationToken cancellationToken)
   {
      var user = await postgresContext.Users
         .FirstOrDefaultAsync(x => x.Id == requestContext.Identity.UserId, cancellationToken);

      if (user is null)
      {
         throw new InternalServerErrorException("User not found");
      }

      user.PasswordHash = argon2Id.HashPassword(request.NewPassword);

      user.MarkAsUpdated(requestContext.Identity.UserId);

      await postgresContext.SaveChangesAsync(cancellationToken);

      BackgroundJob.Enqueue<ISender>(x => x.Send(new RevokeAllUserTokensExceptCurrentV1Command(), cancellationToken));
   }
}
