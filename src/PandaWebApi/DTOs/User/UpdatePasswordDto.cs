using System.Text.Json.Serialization;
using BaseConverter;

namespace PandaWebApi.DTOs.User
{
    public class UpdatePasswordDto
    {
        [JsonConverter(typeof(PandaJsonBaseConverterNotNullable))]
        public long Id { get; set; }
        public string NewPassword { get; set; } = null!;
    }
}