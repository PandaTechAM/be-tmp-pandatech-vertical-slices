using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BaseConverter;
using PandaWebApi.Enums;

namespace PandaWebApi.DTOs.User
{
    public class UpdateUserStatusDto
    {
        [Required]
        [JsonConverter(typeof(PandaJsonBaseConverterNotNullable))]
        public long Id { get; set; }

        [Required] public Statuses Status { get; set; }
    }
}