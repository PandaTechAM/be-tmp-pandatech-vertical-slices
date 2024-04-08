using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pandatech.Crypto;
using Pandatech.VerticalSlices.Features.Auth.Application.RevokeAllTokensExceptCurrentSession;
using Pandatech.VerticalSlices.Infrastructure.Context;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;
using ResponseCrafter.StandardHttpExceptions;

namespace Pandatech.VerticalSlices.Features.Auth.Application.UpdatePasswordForced;

public class UpdatePasswordForcedV1CommandHandler(
   IRequestContext requestContext,
   PostgresContext dbContext,
   Argon2Id argon2Id,
   ISender sender)
   : ICommandHandler<UpdatePasswordForcedV1Command>
{
   public async Task Handle(UpdatePasswordForcedV1Command request, CancellationToken cancellationToken)
   {
      var user = await dbContext.Users
         .FirstOrDefaultAsync(x => x.Id == requestContext.Identity.UserId, cancellationToken);

      if (user is null)
      {
         throw new InternalServerErrorException("User not found");
      }

      var sameWithOldPassword = argon2Id.VerifyHash(request.NewPassword, user.PasswordHash);

      if (sameWithOldPassword)
      {
         throw new BadRequestException("new_password_should_be_different_from_old_password");
      }

      user.PasswordHash = argon2Id.HashPassword(request.NewPassword);
      user.ForcePasswordChange = false;

      user.MarkAsUpdated(requestContext.Identity.UserId);

      await dbContext.SaveChangesAsync(cancellationToken);

      BackgroundJob.Enqueue<ISender>(x => x.Send(new RevokeAllUserTokensExceptCurrentV1Command(), cancellationToken));
   }
}
