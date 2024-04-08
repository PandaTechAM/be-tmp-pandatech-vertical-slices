using Pandatech.VerticalSlices.Domain.Entities;

namespace Pandatech.VerticalSlices.Features.Auth.Contracts.CreateToken;

public record CreateUserTokenV1CommandResponse(
   long Id,
   long UserId,
   string AccessTokenSignature,
   byte[] AccessTokenHash,
   string RefreshTokenSignature,
   byte[] RefreshTokenHash,
   DateTime AccessTokenExpiresAt,
   DateTime RefreshTokenExpiresAt,
   DateTime CreatedAt,
   DateTime UpdatedAt
)
{
   public static CreateUserTokenV1CommandResponse MapFromEntity(UserTokenEntity entity, string accessTokenSignature,
      string refreshTokenSignature)
   {
      return new CreateUserTokenV1CommandResponse(
         entity.Id,
         entity.UserId,
         accessTokenSignature,
         entity.AccessTokenHash,
         refreshTokenSignature,
         entity.RefreshTokenHash,
         entity.AccessTokenExpiresAt,
         entity.RefreshTokenExpiresAt,
         entity.CreatedAt,
         entity.UpdatedAt
      );
   }
}
