using PandaWebApi.Enums;

namespace PandaWebApi.DTOs.Authentication;

public class IdentifyUserDto
{
    public long UserId { get; set; }
    public string Username { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public Roles Role { get; set; }
    public Guid Token { get; set; }
    public DateTime TokenExpirationDate { get; set; }
}