using BaseConverter.Attributes;
using Pandatech.VerticalSlices.Domain.Entities;
using Pandatech.VerticalSlices.Domain.Enums;

namespace Pandatech.VerticalSlices.Features.Auth.Contracts.RefreshToken;

public class RefreshTokenV1CommandResponse
{
   [PropertyBaseConverter] public long UserId { get; set; }
   public bool ForcePasswordChange { get; set; }
   public UserRole UserRole { get; set; }
   public required string AccessTokenSignature { get; set; }
   public DateTime AccessTokenExpiration { get; set; }
   public required string RefreshTokenSignature { get; set; }
   public DateTime RefreshTokenExpiration { get; set; }

   public static RefreshTokenV1CommandResponse MapFromTokenEntity(Token token,
      string accessTokenSignature, string refreshTokenSignature, Token oldToken)
   {
      return new RefreshTokenV1CommandResponse
      {
         UserId = token.UserId,
         ForcePasswordChange = oldToken.User!.ForcePasswordChange,
         UserRole = oldToken.User.Role,
         AccessTokenSignature = accessTokenSignature,
         AccessTokenExpiration = token.AccessTokenExpiresAt,
         RefreshTokenSignature = refreshTokenSignature,
         RefreshTokenExpiration = token.RefreshTokenExpiresAt
      };
   }
}
