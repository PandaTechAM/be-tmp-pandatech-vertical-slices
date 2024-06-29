using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pandatech.VerticalSlices.Domain.Entities;

namespace Pandatech.VerticalSlices.Context.EntityConfigurations;

public class UserConfigEntityConfiguration : IEntityTypeConfiguration<UserConfig>
{
   public void Configure(EntityTypeBuilder<UserConfig> builder)
   {
      builder.HasKey(e => e.Id);
      builder.HasIndex(e => new { e.UserId, e.Key }).IsUnique();
   }
}
