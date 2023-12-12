using System.Text.Json.Serialization;

namespace PandaWebApi.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Roles
{
    SuperAdmin,
    Admin,
    User
}