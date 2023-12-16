using System.Diagnostics.CodeAnalysis;
using PandaWebApi.Enums;

namespace PandaWebApi.DTOs.Authentication;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class IdentifyUserDto
{
    public long Id { get; set; }
    public string Username { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public Roles Role { get; set; } 
    public bool ForcePasswordChange { get; set; }
    public DateTime TokenExpirationDate { get; set; }
}