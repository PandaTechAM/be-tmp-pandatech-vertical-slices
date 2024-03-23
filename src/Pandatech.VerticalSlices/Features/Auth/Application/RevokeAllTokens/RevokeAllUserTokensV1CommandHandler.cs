using Microsoft.EntityFrameworkCore;
using Pandatech.VerticalSlices.Infrastructure.Context;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.Auth.Application.RevokeAllTokens;

public class RevokeAllUserTokensV1CommandHandler(PostgresContext dbContext)
   : ICommandHandler<RevokeAllUserTokensV1Command>
{
   public async Task Handle(RevokeAllUserTokensV1Command request, CancellationToken cancellationToken)
   {
      var now = DateTime.UtcNow;

      var tokens = await dbContext.UserTokens
         .Where(x =>
            x.UserId == request.UserId
            && (x.AccessTokenExpiresAt >= now || x.RefreshTokenExpiresAt >= now))
         .ToListAsync(cancellationToken: cancellationToken);

      if (tokens.Count == 0)
      {
         return;
      }

      foreach (var token in tokens)
      {
         if (token.AccessTokenExpiresAt > now)
         {
            token.AccessTokenExpiresAt = now;
            token.UpdatedAt = now;
         }

         if (token.RefreshTokenExpiresAt > now)
         {
            token.RefreshTokenExpiresAt = now;
            token.UpdatedAt = now;
         }
      }

      await dbContext.SaveChangesAsync(cancellationToken);
   }
}
