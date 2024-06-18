using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pandatech.Crypto;
using Pandatech.VerticalSlices.Context;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.Features.Auth.Application.RevokeAllTokens;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;
using ResponseCrafter.HttpExceptions;

namespace Pandatech.VerticalSlices.Features.User.Application.UpdatePassword;

public class UpdateUserPasswordCommandHandler(
   PostgresContext postgresContext,
   Argon2Id argon2Id,
   IRequestContext requestContext)
   : ICommandHandler<UpdateUserPasswordCommand>
{
   public async Task Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
   {
      var user = await postgresContext
         .Users
         .FirstOrDefaultAsync(u => u.Id == request.Id && u.Role != UserRole.SuperAdmin, cancellationToken);

      if (user is null)
      {
         throw new NotFoundException();
      }

      user.PasswordHash = argon2Id.HashPassword(request.NewPassword);
      user.ForcePasswordChange = true;

      user.MarkAsUpdated(requestContext.Identity.UserId);
      await postgresContext.SaveChangesAsync(cancellationToken);

      BackgroundJob.Enqueue<ISender>(x => x.Send(new RevokeAllTokensCommand(request.Id), cancellationToken));
   }
}
