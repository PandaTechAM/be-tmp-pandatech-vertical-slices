using System.Text.Json.Serialization;

namespace PandaWebApi.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Statuses
    {
        Active,
        Disabled,
        Deleted
    }
}
