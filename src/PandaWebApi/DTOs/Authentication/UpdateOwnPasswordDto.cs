using System.ComponentModel.DataAnnotations;

namespace PandaWebApi.DTOs.Authentication
{
    public class UpdateOwnPasswordDto
    {
        [Required]
        public string OldPassword { get; set; } = null!;
        [Required]
        public string NewPassword { get; set; } = null!;
    }
}