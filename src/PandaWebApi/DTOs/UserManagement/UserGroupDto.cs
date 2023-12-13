using System.ComponentModel.DataAnnotations;

namespace PandaWebApi.DTOs.UserManagement;

public class UserGroupDto
{
    [Required]
    public long GroupId { get; set; }
    [Required]
    public long UserId { get; set; }
}