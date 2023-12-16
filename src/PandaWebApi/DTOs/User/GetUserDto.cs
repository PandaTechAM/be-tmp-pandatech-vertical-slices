using System.Text.Json.Serialization;
using BaseConverter;
using PandaTech.IEnumerableFilters.Attributes;
using PandaWebApi.Enums;

namespace PandaWebApi.DTOs.User
{
    public class GetUserDto
    {
        [JsonConverter(typeof(PandaJsonBaseConverterNotNullable))]
        public long Id { get; set; }
        public string Username { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public RolesSelect Role { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public Statuses Status { get; set; }
        public string? Comment { get; set; } 
    }
}