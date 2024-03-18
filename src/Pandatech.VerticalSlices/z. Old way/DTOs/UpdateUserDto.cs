using System.ComponentModel.DataAnnotations;
using BaseConverter.Attributes;
using Pandatech.VerticalSlices.Domain.Enums;

namespace Pandatech.VerticalSlices.z._Old_way.DTOs
{
    public class UpdateUserDto
    {
        [Required]
        [PandaPropertyBaseConverter]


        public long Id { get; set; }
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string FullName { get; set; } = null!;
        [Required]
        public UserRole UserRole { get; set; }
        public string? Comment { get; set; }
    }
}
