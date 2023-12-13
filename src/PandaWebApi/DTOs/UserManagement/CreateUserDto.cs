using System.ComponentModel.DataAnnotations;
using PandaWebApi.Enums;

namespace PandaWebApi.DTOs.UserManagement
{
    public class CreateUserDto
    {
        [Required] public string Username { get; set; } = null!;
        [Required] public string Password { get; set; } = null!;
        [Required] public string FullName { get; set; } = null!;
        [Required] public Roles Role { get; set; }
        [Required] public string Comment { get; set; } = null!;
        [Required] public long GroupId { get; set; }
    }
}