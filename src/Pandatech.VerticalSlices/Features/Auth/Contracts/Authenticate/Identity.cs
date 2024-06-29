using Pandatech.VerticalSlices.Domain.Enums;

namespace Pandatech.VerticalSlices.Features.Auth.Contracts.Authenticate;

public class Identity
{
   public long UserId { get; set; }
   public string Username { get; set; } = null!;
   public UserStatus Status { get; set; }
   public bool ForcePasswordChange { get; set; }
   public string FullName { get; set; } = null!;
   public UserRole UserRole { get; set; }
   public DateTime CreatedAt { get; set; }
   public DateTime? UpdatedAt { get; set; }
   public long TokenId { get; set; }
   public string AccessTokenSignature { get; set; } = null!;
   public DateTime AccessTokenExpiration { get; set; }
}
