using BaseConverter.Attributes;
using Pandatech.VerticalSlices.Domain.Enums;
using Pandatech.VerticalSlices.Features.Auth.Contracts.CreateToken;

namespace Pandatech.VerticalSlices.Features.Auth.Contracts.Login;

public class LoginCommandResponse
{
   [PandaPropertyBaseConverter] public long UserId { get; set; }

   public bool ForcePasswordChange { get; set; }
   public UserRole UserRole { get; set; }
   public string AccessTokenSignature { get; set; } = null!;
   public DateTime AccessTokenExpiration { get; set; }
   public string RefreshTokenSignature { get; set; } = null!;
   public DateTime RefreshTokenExpiration { get; set; }

   public static LoginCommandResponse MapFromEntity(CreateTokenCommandResponse token, UserRole userRole,
      bool forcePasswordChange)
   {
         return new LoginCommandResponse
         {
            UserId = token.UserId,
            ForcePasswordChange = forcePasswordChange,
            UserRole = userRole,
            AccessTokenSignature = token.AccessTokenSignature,
            AccessTokenExpiration = token.AccessTokenExpiresAt,
            RefreshTokenSignature = token.RefreshTokenSignature,
            RefreshTokenExpiration = token.RefreshTokenExpiresAt
         };
      }
}
