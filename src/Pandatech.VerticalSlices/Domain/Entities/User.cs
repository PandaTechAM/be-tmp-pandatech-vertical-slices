using EFCore.AuditBase;
using Pandatech.VerticalSlices.Domain.Enums;

namespace Pandatech.VerticalSlices.Domain.Entities;

public class User : AuditEntityBase
{
   public long Id { get; set; }
   public required string Username { get; set; }
   public required string FullName { get; set; }
   public required byte[] PasswordHash { get; set; }
   public UserRole Role { get; set; }
   public UserStatus Status { get; set; } = UserStatus.Active;
   public bool ForcePasswordChange { get; set; } = true;
   public string Comment { get; set; }
   public ICollection<Token>? Tokens { get; set; }
}
