using PandaWebApi.Enums;

namespace PandaWebApi.DTOs.Authentication;

public class LoginResponseDto
{
    public long UserId { get; set; }
    public string FullName { get; set; } = null!;
    public Roles Role { get; set; }
    public DateTime TokenExpirationDate { get; set; }
    public bool ForcePasswordChange { get; set; }
}