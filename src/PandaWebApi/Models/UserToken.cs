using Microsoft.EntityFrameworkCore;

namespace PandaWebApi.Models;

[PrimaryKey(nameof(Id))]
[Index(nameof(AccessTokenHash))]
[Index(nameof(RefreshTokenHash))]
public class UserToken
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long? PreviousUserTokenId { get; set; }
    public byte[] AccessTokenHash { get; set; } = null!;
    public byte[] RefreshTokenHash { get; set; } = null!;
    public DateTime AccessTokenExpiresAt { get; set; }
    public DateTime RefreshTokenExpiresAt { get; set; }
    public DateTime InitialRefreshTokenCreatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public User User { get; set; } = null!;
        
    public UserToken PreviousUserToken { get; set; } = null!;
}