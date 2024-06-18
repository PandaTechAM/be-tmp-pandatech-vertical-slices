using Microsoft.EntityFrameworkCore;
using Pandatech.VerticalSlices.Context;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;

namespace Pandatech.VerticalSlices.Features.Auth.Application.RevokeAllTokens;

public class RevokeAllTokensCommandHandler(PostgresContext dbContext)
   : ICommandHandler<RevokeAllTokensCommand>
{
   public async Task Handle(RevokeAllTokensCommand request, CancellationToken cancellationToken)
   {
         var now = DateTime.UtcNow;

         var tokens = await dbContext.Tokens
            .Where(x =>
               x.UserId == request.UserId
               && (x.AccessTokenExpiresAt >= now || x.RefreshTokenExpiresAt >= now))
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