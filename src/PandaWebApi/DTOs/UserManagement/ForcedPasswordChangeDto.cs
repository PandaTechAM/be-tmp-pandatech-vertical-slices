using System.ComponentModel.DataAnnotations;

namespace PandaWebApi.DTOs.UserManagement
{
    public class ForcedPasswordChangeDto
    {
        [Required]
        public string Password { get; set; } = null!;
    }
}