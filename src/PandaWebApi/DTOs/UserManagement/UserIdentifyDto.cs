using System.ComponentModel.DataAnnotations;

namespace PandaWebApi.DTOs.UserManagement
{
    public class UserIdentifyDto
    {
        [Required] 
        public Guid Token { get; set; } 
    }
}