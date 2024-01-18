using Microsoft.EntityFrameworkCore;
using PandaTech.IEnumerableFilters.Attributes;
using PandaWebApi.Enums;
using PandaWebApi.FilterModels;

namespace PandaWebApi.Models;

[FilterModel(typeof(UserFilter))]
[PrimaryKey(nameof(Id))]
[Index(nameof(Username), IsUnique = true)]
[Index(nameof(FullName))]
public class User
{
    public long Id { get; set; }
    public string Username { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public byte[] PasswordHash { get; set; } = null!;
    public Roles Role { get; set; }
    public Statuses Status { get; set; }
    public bool ForcePasswordChange { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? Comment { get; set; }

    public ICollection<UserAuthenticationHistory>? UserAuthenticationHistories { get; set; }
}