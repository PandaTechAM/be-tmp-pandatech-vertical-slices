using System.ComponentModel.DataAnnotations;

namespace PandaWebApi.DTOs.UserToken;

public class RefreshTokenDto
{
    [Required] public string RefreshToken { get; set; } = null!;
}