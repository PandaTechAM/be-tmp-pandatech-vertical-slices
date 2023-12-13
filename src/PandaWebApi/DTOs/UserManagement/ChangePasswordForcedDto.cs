using System.ComponentModel.DataAnnotations;

namespace PandaWebApi.DTOs.UserManagement;

public class ChangePasswordForcedDto
{
    [Required] public string NewPassword { get; set; } = null!;
}