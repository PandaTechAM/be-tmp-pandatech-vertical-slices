using System.ComponentModel.DataAnnotations;
using PandaWebApi.Enums;

namespace PandaWebApi.DTOs.UserManagement
{
    public class UpdateUserDto
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string FullName { get; set; } = null!;
        [Required]
        public Roles Role { get; set; }
        public string? Comment { get; set; }
    }
}