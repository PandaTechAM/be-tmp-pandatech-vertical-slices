using System.ComponentModel.DataAnnotations;

namespace PandaWebApi.DTOs.Authentication
{
    public class UpdatePasswordForced
    {
        [Required]
        public string Password { get; set; } = null!;
    }
}