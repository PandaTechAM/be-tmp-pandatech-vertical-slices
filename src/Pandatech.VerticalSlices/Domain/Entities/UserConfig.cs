using EFCore.AuditBase;

namespace Pandatech.VerticalSlices.Domain.Entities;

public class UserConfig : AuditEntityBase
{
   public long Id { get; set; }
   public long UserId { get; set; }
   public required string Key { get; set; }
   public required string Value { get; set; }
   public User? User { get; set; }
}