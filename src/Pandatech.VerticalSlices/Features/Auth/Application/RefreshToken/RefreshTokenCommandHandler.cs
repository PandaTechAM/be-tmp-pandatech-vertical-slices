using Microsoft.EntityFrameworkCore;
using Pandatech.Crypto;
using Pandatech.VerticalSlices.Context;
using Pandatech.VerticalSlices.Domain.Entities;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.Features.Auth.Contracts.RefreshToken;
using Pandatech.VerticalSlices.Features.Auth.Helpers;
using Pandatech.VerticalSlices.SharedKernel.Helpers;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;
using ResponseCrafter.HttpExceptions;

namespace Pandatech.VerticalSlices.Features.Auth.Application.RefreshToken;

public class RefreshTokenCommandHandler(IConfiguration configuration, PostgresContext dbContext)
   : ICommandHandler<RefreshTokenCommand, RefreshTokenV1CommandResponse>
{
   private const int AccessTokenExpirationMinutes = TokenHelpers.AccessTokenExpirationMinutes;

   private readonly int _refreshTokenExpirationMinutes =
      TokenHelpers.SetRefreshTokenExpirationMinutes(configuration);

   private readonly int _refreshTokenMaxExpirationMinutes =
      TokenHelpers.SetRefreshTokenMaxExpirationMinutes(configuration);

   public async Task<RefreshTokenV1CommandResponse> Handle(RefreshTokenCommand request,
      CancellationToken cancellationToken)
   {
      var now = DateTime.UtcNow;

      var refreshTokenHash = Sha3.Hash(request.RefreshTokenSignature);

      var token = await dbContext.Tokens
         .Include(ut => ut.User)
         .FirstOrDefaultAsync(x => x.RefreshTokenHash == refreshTokenHash, cancellationToken);

      ValidateToken(token, now);

      var newToken =
         CreateNewToken(now, token, out var newRefreshTokenSignature, out var accessTokenSignature);

      dbContext.Tokens.Add(newToken);
      InvalidateOldToken(token, now);
      await dbContext.SaveChangesAsync(cancellationToken);
      return RefreshTokenV1CommandResponse.MapFromTokenEntity(newToken, accessTokenSignature,
         newRefreshTokenSignature, token!);
   }

   private static void ValidateToken(Token? token, DateTime now)
   {
      NotFoundException.ThrowIfNull(token);

      UnauthorizedException.ThrowIf(token.User.Status != UserStatus.Active,
         ErrorMessages.ThisUserIsNotAllowedToPerformThisAction);

      UnauthorizedException.ThrowIf(token.RefreshTokenExpiresAt < now, ErrorMessages.RefreshTokenExpired);
   }

   private Token CreateNewToken(DateTime now, Token? token, out string refreshTokenSignature,
      out string accessTokenSignature)
   {
      var newExpirationDate = now.AddMinutes(_refreshTokenExpirationMinutes);

      if (newExpirationDate > token!.InitialRefreshTokenCreatedAt.AddMinutes(_refreshTokenMaxExpirationMinutes))
      {
         newExpirationDate = token.InitialRefreshTokenCreatedAt.AddMinutes(_refreshTokenMaxExpirationMinutes);
      }

      if (newExpirationDate <= now.AddMinutes(60))
      {
         newExpirationDate = now.AddMinutes(60);
      }

      accessTokenSignature = Guid.NewGuid().ToString();
      refreshTokenSignature = Guid.NewGuid().ToString();

      return new Token
      {
         UserId = token.UserId,
         PreviousTokenId = token.Id,
         AccessTokenHash = Sha3.Hash(accessTokenSignature),
         RefreshTokenHash = Sha3.Hash(refreshTokenSignature),
         AccessTokenExpiresAt = now.AddMinutes(AccessTokenExpirationMinutes),
         RefreshTokenExpiresAt = newExpirationDate,
         InitialRefreshTokenCreatedAt = token.InitialRefreshTokenCreatedAt,
         CreatedAt = now,
         UpdatedAt = now
      };
   }

   private static void InvalidateOldToken(Token? token, DateTime now)
   {
      token!.RefreshTokenExpiresAt = now;
      token.UpdatedAt = now;
      if (token.AccessTokenExpiresAt > now)
      {
         token.AccessTokenExpiresAt = now;
      }
   }
}
