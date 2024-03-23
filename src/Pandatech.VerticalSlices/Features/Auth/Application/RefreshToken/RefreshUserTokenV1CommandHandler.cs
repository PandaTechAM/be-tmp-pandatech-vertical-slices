using Microsoft.EntityFrameworkCore;
using Pandatech.Crypto;
using Pandatech.VerticalSlices.Domain.Entities;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.Features.Auth.Contracts.RefreshToken;
using Pandatech.VerticalSlices.Features.Auth.Helpers;
using Pandatech.VerticalSlices.Infrastructure.Context;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;
using ResponseCrafter.StandardHttpExceptions;

namespace Pandatech.VerticalSlices.Features.Auth.Application.RefreshToken;

public class RefreshUserTokenV1CommandHandler(IConfiguration configuration, PostgresContext dbContext)
   : ICommandHandler<RefreshUserTokenV1Command, RefreshUserTokenV1CommandResponse>
{
   private const int AccessTokenExpirationMinutes = UserTokenHelpers.AccessTokenExpirationMinutes;

   private readonly int _refreshTokenExpirationMinutes =
      UserTokenHelpers.SetRefreshTokenExpirationMinutes(configuration);

   private readonly int _refreshTokenMaxExpirationMinutes =
      UserTokenHelpers.SetRefreshTokenMaxExpirationMinutes(configuration);

   public async Task<RefreshUserTokenV1CommandResponse> Handle(RefreshUserTokenV1Command request,
      CancellationToken cancellationToken)
   {
      var now = DateTime.UtcNow;

      var refreshTokenHash = Sha3.Hash(request.RefreshTokenSignature);

      var userToken = await dbContext.UserTokens
         .Include(ut => ut.User)
         .FirstOrDefaultAsync(x => x.RefreshTokenHash == refreshTokenHash, cancellationToken);

      ValidateUserToken(userToken, now);

      var newToken =
         CreateNewToken(now, userToken, out var newRefreshTokenSignature, out var accessTokenSignature);

      await dbContext.UserTokens.AddAsync(newToken, cancellationToken);
      InvalidateOldToken(userToken, now);
      await dbContext.SaveChangesAsync(cancellationToken);
      return RefreshUserTokenV1CommandResponse.MapFromUserTokenEntity(newToken, accessTokenSignature,
         newRefreshTokenSignature, userToken!);
   }

   private static void ValidateUserToken(UserTokenEntity? userToken, DateTime now)
   {
      if (userToken == null)
      {
         throw new NotFoundException("refresh_token_is_invalid");
      }

      if (userToken.User.Status != UserStatus.Active)
      {
         throw new UnauthorizedException("you_cannot_refresh_token_with_this_user");
      }

      if (userToken.RefreshTokenExpiresAt < now)
      {
         throw new UnauthorizedException("refresh_token_has_expired");
      }
   }

   private UserTokenEntity CreateNewToken(DateTime now, UserTokenEntity? userToken, out string refreshTokenSignature,
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

      return new UserTokenEntity
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

   private static void InvalidateOldToken(UserTokenEntity? userToken, DateTime now)
   {
      userToken!.RefreshTokenExpiresAt = now;
      userToken.UpdatedAt = now;
      if (userToken.AccessTokenExpiresAt > now)
      {
         userToken.AccessTokenExpiresAt = now;
      }
   }
}
