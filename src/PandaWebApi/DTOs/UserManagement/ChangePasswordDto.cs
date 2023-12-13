namespace PandaWebApi.DTOs.UserManagement
{
    public class ChangePasswordDto
    {
        public long Id { get; set; }
        public string NewPassword { get; set; } = null!;
    }
}