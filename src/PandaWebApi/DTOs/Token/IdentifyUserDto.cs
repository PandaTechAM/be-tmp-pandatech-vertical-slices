using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using BaseConverter;
using PandaWebApi.Enums;

namespace PandaWebApi.DTOs.Token;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class IdentifyUserDto
{
    public long Id { get; set; }

    public string Username { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public Roles Role { get; set; }
    public bool ForcePasswordChange { get; set; }
}