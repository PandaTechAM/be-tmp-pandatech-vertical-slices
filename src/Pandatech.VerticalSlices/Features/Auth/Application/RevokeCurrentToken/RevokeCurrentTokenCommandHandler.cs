using Microsoft.EntityFrameworkCore;
using Pandatech.VerticalSlices.Context;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;
using ResponseCrafter.HttpExceptions;

namespace Pandatech.VerticalSlices.Features.Auth.Application.RevokeCurrentToken;

public class RevokeCurrentTokenCommandHandler(IRequestContext requestContext, PostgresContext dbContext)
   : ICommandHandler<RevokeCurrentTokenCommand>
{
   public async Task Handle(RevokeCurrentTokenCommand request, CancellationToken cancellationToken)
   {
      var now = DateTime.UtcNow;

      var token = await dbContext.Tokens
         .FirstOrDefaultAsync(x => x.Id == requestContext.Identity.UserTokenId, cancellationToken);

      if (token is null)
      {
         throw new NotFoundException();
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
