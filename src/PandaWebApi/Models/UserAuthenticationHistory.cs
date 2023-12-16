using Microsoft.EntityFrameworkCore;

namespace PandaWebApi.Models;

[PrimaryKey(nameof(Id))]
[Index(nameof(CreatedAt))]
public class UserAuthenticationHistory
{
    public long Id { get; set; }

    public User? User { get; set; }
    public long? UserId { get; set; }

    public DateTime CreatedAt { get; set; }
    public bool IsAuthenticated { get; set; }
}