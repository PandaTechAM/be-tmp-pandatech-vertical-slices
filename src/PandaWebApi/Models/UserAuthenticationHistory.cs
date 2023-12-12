using System.ComponentModel.DataAnnotations;

namespace PandaWebApi.Models;

public class UserAuthenticationHistory
{
    [Key]
    public long Id { get; set; }

    public User? User { get; set; } = null!;
    public long? UserId { get; set; }

    public DateTime AuthenticationDate { get; set; }
    public bool IsAuthenticated { get; set; }
}