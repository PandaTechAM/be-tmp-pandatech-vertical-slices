using Microsoft.EntityFrameworkCore;
using Pandatech.Crypto;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.Features.Auth.Contracts.Authenticate;
using Pandatech.VerticalSlices.Infrastructure.Contexts;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;
using ResponseCrafter.StandardHttpExceptions;

namespace Pandatech.VerticalSlices.Features.Auth.Application.Authenticate;

public class AuthenticateV1QueryHandler(PostgresContext dbContext)
   : IQueryHandler<AuthenticateV1Query, Identity>
{
   public async Task<Identity> Handle(AuthenticateV1Query request, CancellationToken cancellationToken)
   {
      var accessTokenHash = Sha3.Hash(request.AccessTokenSignature);

      var tokenEntity = await dbContext.UserTokens
         .Include(ut => ut.User)
         .Where(t => t.AccessTokenHash == accessTokenHash)
         .AsNoTracking()
         .FirstOrDefaultAsync(cancellationToken);

      if (tokenEntity is null || tokenEntity.User.Status is not UserStatus.Active)
      {
         throw new UnauthorizedException();
      }
      
      if (tokenEntity.AccessTokenExpiresAt <= DateTime.UtcNow)
      {
         throw new UnauthorizedException("access_token_is_expired");
      }

      return new Identity
      {
         UserId = tokenEntity.UserId,
         Status = tokenEntity.User.Status,
         ForcePasswordChange = tokenEntity.User.ForcePasswordChange,
         FullName = tokenEntity.User.FullName,
         UserRole = tokenEntity.User.Role,
         CreatedAt = tokenEntity.User.CreatedAt,
         UpdatedAt = tokenEntity.User.UpdatedAt,
         UserTokenId = tokenEntity.Id,
         AccessTokenSignature = request.AccessTokenSignature,
         AccessTokenExpiration = tokenEntity.AccessTokenExpiresAt
      };

   }
}
