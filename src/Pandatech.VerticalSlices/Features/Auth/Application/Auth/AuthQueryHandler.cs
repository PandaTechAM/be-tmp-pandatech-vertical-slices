using Microsoft.EntityFrameworkCore;
using Pandatech.Crypto;
using Pandatech.VerticalSlices.Context;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.Features.Auth.Contracts.Authenticate;
using Pandatech.VerticalSlices.Features.Auth.Helpers;
using Pandatech.VerticalSlices.Features.Auth.Helpers.ApiAuth;
using Pandatech.VerticalSlices.SharedKernel.Helpers;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;
using ResponseCrafter.HttpExceptions;

namespace Pandatech.VerticalSlices.Features.Auth.Application.Auth;

public class AuthQueryHandler(PostgresContext dbContext, IHostEnvironment environment, IRequestContext requestContext)
   : IQueryHandler<AuthQuery>
{
   public async Task Handle(AuthQuery request, CancellationToken cancellationToken)
   {
      var now = DateTime.UtcNow;
      var requestId = request.HttpContext.TryParseRequestId();
      var clientType = request.HttpContext.TryParseClientType().ConvertToEnum(!request.IgnoreClientType);
      var accessTokenSignature = request.HttpContext.TryParseAccessTokenSignature(environment);

      var metadata = new MetaData { RequestId = requestId, RequestTime = now, ClientType = clientType };

      requestContext.MetaData = metadata;

      if (request.Anonymous)
      {
         return;
      }

      if (string.IsNullOrWhiteSpace(accessTokenSignature))
      {
         throw new UnauthorizedException(ErrorMessages.AccessTokenIsRequired);
      }

      var accessTokenHash = Sha3.Hash(accessTokenSignature);

      var tokenEntity = await dbContext.Tokens
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
         throw new UnauthorizedException(ErrorMessages.AccessTokenIsExpired);
      }

      var identity = new Identity
      {
         UserId = tokenEntity.UserId,
         Status = tokenEntity.User.Status,
         ForcePasswordChange = tokenEntity.User.ForcePasswordChange,
         FullName = tokenEntity.User.FullName,
         UserRole = tokenEntity.User.Role,
         CreatedAt = tokenEntity.User.CreatedAt,
         UpdatedAt = tokenEntity.User.UpdatedAt,
         UserTokenId = tokenEntity.Id,
         AccessTokenSignature = accessTokenSignature,
         AccessTokenExpiration = tokenEntity.AccessTokenExpiresAt
      };

      AuthorizationHelper.Authorize(identity, request.MinimalUserRole, request.ForcedToChangePassword);

      requestContext.Identity = identity;
      requestContext.IsAuthenticated = true;
   }
}
