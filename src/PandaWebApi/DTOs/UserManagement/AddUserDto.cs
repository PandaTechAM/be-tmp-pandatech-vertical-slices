using System.ComponentModel.DataAnnotations;
using PandaWebApi.Enums;

namespace PandaWebApi.DTOs.UserManagement
{
    public class AddUserDto
    {
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!; 
        [Required]
        public string FullName { get; set; } = null!;
        [Required]
        public Roles Role { get; set; }
        public string? Comment { get; set; }
    }
}