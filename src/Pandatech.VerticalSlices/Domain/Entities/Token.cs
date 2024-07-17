namespace Pandatech.VerticalSlices.Domain.Entities;

public class Token
{
   public long Id { get; set; }
   public long UserId { get; set; }
   public long? PreviousTokenId { get; set; }
   public required byte[] AccessTokenHash { get; set; }
   public required byte[] RefreshTokenHash { get; set; }
   public DateTime AccessTokenExpiresAt { get; set; }
   public DateTime RefreshTokenExpiresAt { get; set; }
   public DateTime InitialRefreshTokenCreatedAt { get; set; } = DateTime.UtcNow;
   public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
   public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
   public User? User { get; set; }
   public Token? PreviousToken { get; set; }
}