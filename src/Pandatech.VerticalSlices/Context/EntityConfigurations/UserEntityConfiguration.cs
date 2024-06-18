using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pandatech.VerticalSlices.Domain.Entities;

namespace Pandatech.VerticalSlices.Context.EntityConfigurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
   public void Configure(EntityTypeBuilder<User> builder)
   {
      builder.HasKey(b => b.Id);
      builder.HasIndex(b => b.Username).IsUnique();
      builder.HasIndex(b => b.FullName);
   }
}
