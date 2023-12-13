using PandaWebApi.Enums;

namespace PandaWebApi.DTOs.UserManagement;

public class IdentifyUserDto
{
    public long Id { get; set; }
    public Guid Token { get; set; }
    public string Username { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public bool ForceToChangePassword { get; set; }
    public DateTime CreationDate { get; set; }
    public List<string> Groups { get; set; } = null!;
    
    public Roles Role { get; set; }
}