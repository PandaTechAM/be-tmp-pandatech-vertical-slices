using BaseConverter.Attributes;
using Pandatech.VerticalSlices.Domain.Enums;

namespace Pandatech.VerticalSlices.z._Old_way.DTOs
{
    public class GetUserDto
    {
        [PandaPropertyBaseConverter]
        public long Id { get; set; }
        public string Username { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public RolesSelect Role { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public UserStatus Status { get; set; }
        public string? Comment { get; set; } 
    }
}
