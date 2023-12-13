
using PandaWebApi.Enums;

namespace PandaWebApi.DTOs.UserManagement
{
    public class LogInResponseDto
    {
        public long Id { get; set; } 
        public Guid Token { get; set; }
        public string FullName { get; set; } = null!;
        public string Username { get; set; } = null!;
        public bool ForceToChangePassword { get; set; }

        public Roles Role { get; set; }
    }
}
