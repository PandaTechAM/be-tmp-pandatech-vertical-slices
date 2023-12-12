using System.ComponentModel.DataAnnotations;

namespace PandaWebApi.DTOs.Authentication;

public class LoginDto
{
    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}