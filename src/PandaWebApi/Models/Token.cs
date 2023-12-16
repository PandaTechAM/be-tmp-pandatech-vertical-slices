using Microsoft.EntityFrameworkCore;

namespace PandaWebApi.Models;

[PrimaryKey(nameof(Id))]
[Index(nameof(SignatureHash))]
[Index(nameof(ExpirationDate))]
public class Token
{
    public long Id { get; set; }
    public byte[] SignatureHash { get; set; } = null!;

    public User User { get; set; } = null!;
    public long UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpirationDate { get; set; }
}