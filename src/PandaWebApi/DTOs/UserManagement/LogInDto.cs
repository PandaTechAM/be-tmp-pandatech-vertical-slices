using System.ComponentModel.DataAnnotations;

namespace PandaWebApi.DTOs.UserManagement
{
    public class LogInDto
    {
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
