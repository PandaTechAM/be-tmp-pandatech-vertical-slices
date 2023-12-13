using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using PandaWebApi.Enums;

namespace PandaWebApi.Models;

[Index(nameof(Username), IsUnique = true)]
public class User
{
    [Key]
    public long Id { get; set; }

    public string Username { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public byte[] PasswordHash { get; set; } = null!;
    public Roles Role { get; set; }
    public DateTime CreationDate { get; set; }
    public Statuses Status { get; set; }
    public bool ForcePasswordChange { get; set; }
    public string? Comment { get; set; }
}