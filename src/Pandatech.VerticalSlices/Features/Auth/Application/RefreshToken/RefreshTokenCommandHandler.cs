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

      var userToken = await dbContext.Tokens
         .Include(ut => ut.User)
         .FirstOrDefaultAsync(x => x.RefreshTokenHash == refreshTokenHash, cancellationToken);

      ValidateUserToken(userToken, now);

      var newToken =
         CreateNewToken(now, userToken, out var newRefreshTokenSignature, out var accessTokenSignature);

      dbContext.Tokens.Add(newToken);
      InvalidateOldToken(userToken, now);
      await dbContext.SaveChangesAsync(cancellationToken);
      return RefreshTokenV1CommandResponse.MapFromUserTokenEntity(newToken, accessTokenSignature,
         newRefreshTokenSignature, userToken!);
   }

   private static void ValidateUserToken(Token? userToken, DateTime now)
   {
      if (userToken == null)
      {
         throw new NotFoundException();
      }

      if (userToken.User.Status != UserStatus.Active)
      {
         throw new UnauthorizedException(ErrorMessages.ThisUserIsNotAllowedToPerformThisAction);
      }

      if (userToken.RefreshTokenExpiresAt < now)
      {
         throw new UnauthorizedException(ErrorMessages.RefreshTokenExpired);
      }
   }

   private Token CreateNewToken(DateTime now, Token? userToken, out string refreshTokenSignature,
      out string accessTokenSignature)
   {
      var newExpirationDate = now.AddMinutes(_refreshTokenExpirationMinutes);

      if (newExpirationDate > userToken!.InitialRefreshTokenCreatedAt.AddMinutes(_refreshTokenMaxExpirationMinutes))
      {
         newExpirationDate = userToken.InitialRefreshTokenCreatedAt.AddMinutes(_refreshTokenMaxExpirationMinutes);
      }

      if (newExpirationDate <= now.AddMinutes(60))
      {
         newExpirationDate = now.AddMinutes(60);
      }

      accessTokenSignature = Guid.NewGuid().ToString();
      refreshTokenSignature = Guid.NewGuid().ToString();

      return new Token
      {
         UserId = userToken.UserId,
         PreviousUserTokenId = userToken.Id,
         AccessTokenHash = Sha3.Hash(accessTokenSignature),
         RefreshTokenHash = Sha3.Hash(refreshTokenSignature),
         AccessTokenExpiresAt = now.AddMinutes(AccessTokenExpirationMinutes),
         RefreshTokenExpiresAt = newExpirationDate,
         InitialRefreshTokenCreatedAt = userToken.InitialRefreshTokenCreatedAt,
         CreatedAt = now,
         UpdatedAt = now
      };
   }

   private static void InvalidateOldToken(Token? userToken, DateTime now)
   {
      userToken!.RefreshTokenExpiresAt = now;
      userToken.UpdatedAt = now;
      if (userToken.AccessTokenExpiresAt > now)
      {
         userToken.AccessTokenExpiresAt = now;
      }
   }
}
