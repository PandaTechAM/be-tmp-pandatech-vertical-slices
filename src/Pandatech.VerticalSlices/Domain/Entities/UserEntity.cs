using PandaTech.IEnumerableFilters.Attributes;
using Pandatech.VerticalSlices.Domain.EntityFilters;
using Pandatech.VerticalSlices.Domain.Enums;

namespace Pandatech.VerticalSlices.Domain.Entities;

[FilterModel(typeof(UserEntityFilter))]
public class UserEntity
{
  public long Id { get; set; }
  public string Username { get; set; } = null!;
  public string FullName { get; set; } = null!;
  public byte[] PasswordHash { get; set; } = null!;
  public UserRole Role { get; set; }
  public UserStatus Status { get; set; } = UserStatus.Active;
  public bool ForcePasswordChange { get; set; } = true;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
  public string? Comment { get; set; }

  public ICollection<UserTokenEntity> UserTokens { get; set; } = new List<UserTokenEntity>();
}
