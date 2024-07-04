using Pandatech.VerticalSlices.Domain.Enums;

namespace Pandatech.VerticalSlices.Features.Auth.Contracts.Authenticate;

public class Identity
{
   public long UserId { get; set; }
   public required string Username { get; set; }
   public UserStatus Status { get; set; }
   public bool ForcePasswordChange { get; set; }
   public required string FullName { get; set; }
   public UserRole UserRole { get; set; }
   public DateTime CreatedAt { get; set; }
   public DateTime? UpdatedAt { get; set; }
   public long TokenId { get; set; }
   public required string AccessTokenSignature { get; set; }
   public DateTime AccessTokenExpiration { get; set; }
}
