using Microsoft.EntityFrameworkCore;
using Pandatech.VerticalSlices.Infrastructure.Context;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;
using ResponseCrafter.HttpExceptions;

namespace Pandatech.VerticalSlices.Features.Auth.Application.RevokeCurrentToken;

public class RevokeCurrentTokenV1CommandHandler(IRequestContext requestContext, PostgresContext dbContext)
   : ICommandHandler<RevokeCurrentTokenV1Command>
{
   public async Task Handle(RevokeCurrentTokenV1Command request, CancellationToken cancellationToken)
   {
      var now = DateTime.UtcNow;

      var token = await dbContext.UserTokens
         .FirstOrDefaultAsync(x => x.Id == requestContext.Identity.UserTokenId, cancellationToken);

      if (token is null)
      {
         throw new NotFoundException("Token not found");
      }

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

      await dbContext.SaveChangesAsync(cancellationToken);
   }
}
