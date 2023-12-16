using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BaseConverter;
using PandaWebApi.Enums;

namespace PandaWebApi.DTOs.User
{
    public class UpdateUserDto
    {
        [Required]
        [JsonConverter(typeof(PandaJsonBaseConverterNotNullable))]

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