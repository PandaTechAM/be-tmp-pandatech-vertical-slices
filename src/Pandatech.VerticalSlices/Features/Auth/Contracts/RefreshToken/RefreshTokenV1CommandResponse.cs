using BaseConverter.Attributes;
using Pandatech.VerticalSlices.Domain.Entities;
using Pandatech.VerticalSlices.Domain.Enums;

namespace Pandatech.VerticalSlices.Features.Auth.Contracts.RefreshToken;

public class RefreshTokenV1CommandResponse
{
   [PropertyBaseConverter] public long UserId { get; set; }
   public bool ForcePasswordChange { get; set; }
   public UserRole UserRole { get; set; }
   public string AccessTokenSignature { get; set; } = null!;
   public DateTime AccessTokenExpiration { get; set; }
   public string RefreshTokenSignature { get; set; } = null!;
   public DateTime RefreshTokenExpiration { get; set; }

   public static RefreshTokenV1CommandResponse MapFromTokenEntity(Token token,
      string accessTokenSignature, string refreshTokenSignature, Token oldToken)
   {
      return new RefreshTokenV1CommandResponse
      {
         UserId = token.UserId,
         ForcePasswordChange = oldToken.User.ForcePasswordChange,
         UserRole = oldToken.User.Role,
         AccessTokenSignature = accessTokenSignature,
         AccessTokenExpiration = token.AccessTokenExpiresAt,
         RefreshTokenSignature = refreshTokenSignature,
         RefreshTokenExpiration = token.RefreshTokenExpiresAt
      };
   }
}
