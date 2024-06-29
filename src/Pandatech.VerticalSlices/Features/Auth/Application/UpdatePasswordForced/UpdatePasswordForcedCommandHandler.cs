using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pandatech.Crypto;
using Pandatech.VerticalSlices.Context;
using Pandatech.VerticalSlices.Features.Auth.Application.RevokeAllTokensExceptCurrentSession;
using Pandatech.VerticalSlices.SharedKernel.Helpers;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;
using ResponseCrafter.HttpExceptions;

namespace Pandatech.VerticalSlices.Features.Auth.Application.UpdatePasswordForced;

public class UpdatePasswordForcedCommandHandler(
   IRequestContext requestContext,
   PostgresContext dbContext,
   Argon2Id argon2Id)
   : ICommandHandler<UpdatePasswordForcedCommand>
{
   public async Task Handle(UpdatePasswordForcedCommand request, CancellationToken cancellationToken)
   {
      var user = await dbContext.Users
         .FirstOrDefaultAsync(x => x.Id == requestContext.Identity.UserId, cancellationToken);

      InternalServerErrorException.ThrowIfNull(user, "User not found");

      var sameWithOldPassword = argon2Id.VerifyHash(request.NewPassword, user.PasswordHash);

      BadRequestException.ThrowIf(sameWithOldPassword, ErrorMessages.NewPasswordMustBeDifferentFromOldPassword);

      user.PasswordHash = argon2Id.HashPassword(request.NewPassword);
      user.ForcePasswordChange = false;

      user.MarkAsUpdated(requestContext.Identity.UserId);

      await dbContext.SaveChangesAsync(cancellationToken);

      BackgroundJob.Enqueue<ISender>(x => x.Send(new RevokeAllTokensExceptCurrentCommand(), cancellationToken));
   }
}
