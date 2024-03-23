using Hangfire;
using MediatR;
using Pandatech.Crypto;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.Features.Auth.Application.RevokeAllTokens;
using Pandatech.VerticalSlices.Infrastructure.Context;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;
using ResponseCrafter.StandardHttpExceptions;

namespace Pandatech.VerticalSlices.Features.User.Application.UpdatePassword;

public class UpdateUserPasswordV1CommandHandler(PostgresContext postgresContext, Argon2Id argon2Id)
   : ICommandHandler<UpdateUserPasswordV1Command>
{
   public async Task Handle(UpdateUserPasswordV1Command request, CancellationToken cancellationToken)
   {
      var user = await postgresContext.Users.FindAsync([request.Id], cancellationToken);

      if (user is null || user.Role == UserRole.SuperAdmin)
      {
         throw new NotFoundException("User not found");
      }

      user.PasswordHash = argon2Id.HashPassword(request.NewPassword);
      user.ForcePasswordChange = true;
      user.UpdatedAt = DateTime.UtcNow;

      await postgresContext.SaveChangesAsync(cancellationToken);

      BackgroundJob.Enqueue<ISender>(x => x.Send(new RevokeAllUserTokensV1Command(request.Id), cancellationToken));
   }
}
