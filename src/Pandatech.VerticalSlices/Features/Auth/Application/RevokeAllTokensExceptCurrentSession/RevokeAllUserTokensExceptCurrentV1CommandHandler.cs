using Microsoft.EntityFrameworkCore;
using Pandatech.VerticalSlices.Infrastructure.Contexts;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.Auth.Application.RevokeAllTokensExceptCurrentSession;

public class RevokeAllUserTokensExceptCurrentV1CommandHandler(PostgresContext dbContext, IRequestContext requestContext)
   : ICommandHandler<RevokeAllUserTokensExceptCurrentV1Command>
{
   public async Task Handle(RevokeAllUserTokensExceptCurrentV1Command request, CancellationToken cancellationToken)
   {
      var now = DateTime.UtcNow;

      var tokens = await dbContext.UserTokens
         .Where(x => x.UserId == requestContext.Identity.UserId && x.Id != requestContext.Identity.UserTokenId)
         .ToListAsync(cancellationToken);

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
