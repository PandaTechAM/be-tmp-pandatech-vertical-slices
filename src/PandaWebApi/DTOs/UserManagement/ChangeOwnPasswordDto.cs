using System.ComponentModel.DataAnnotations;

namespace PandaWebApi.DTOs.UserManagement
{
    public class ChangeOwnPasswordDto
    {
        [Required]
        public string OldPassword { get; set; } = null!;
        [Required]
        public string NewPassword { get; set; } = null!;
    }
}