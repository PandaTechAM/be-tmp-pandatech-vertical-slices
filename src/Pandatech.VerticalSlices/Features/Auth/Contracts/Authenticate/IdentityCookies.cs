namespace Pandatech.VerticalSlices.Features.Auth.Contracts.Authenticate;

public class IdentityCookies
{
   public required string AccessTokenSignature { get; set; } 
   public required string RefreshTokenSignature { get; set; } 
   public DateTime AccessTokenExpiresAt { get; set; }
   public DateTime RefreshTokenExpiresAt { get; set; }
}
