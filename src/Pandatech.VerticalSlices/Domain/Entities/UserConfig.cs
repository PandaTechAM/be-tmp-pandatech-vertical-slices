using EFCore.AuditBase;

namespace Pandatech.VerticalSlices.Domain.Entities;

public class UserConfig : AuditEntityBase
{
   public long Id { get; set; }
   public long UserId { get; set; }
   public string Key { get; set; } = null!;
   public string Value { get; set; } = null!;
   public User User { get; set; } = null!;
}
