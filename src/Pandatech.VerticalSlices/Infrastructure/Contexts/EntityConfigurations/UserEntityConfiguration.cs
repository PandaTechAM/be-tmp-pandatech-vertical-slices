using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pandatech.VerticalSlices.Domain.Entities;

namespace Pandatech.VerticalSlices.Infrastructure.Contexts.EntityConfigurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
  public void Configure(EntityTypeBuilder<UserEntity> builder)
  {
    builder.HasKey(b => b.Id);

    builder.Property(b => b.Username).IsRequired();
    builder.Property(b => b.FullName).IsRequired();
    builder.Property(b => b.PasswordHash).IsRequired();
    builder.Property(b => b.Role).IsRequired();
    builder.Property(b => b.Status).IsRequired();
    builder.Property(b => b.ForcePasswordChange).IsRequired();
    builder.Property(b => b.CreatedAt).IsRequired();
    builder.Property(b => b.UpdatedAt).IsRequired();
    builder.Property(b => b.Comment);

    builder.HasMany(b => b.UserTokens)
      .WithOne(u => u.User)
      .HasForeignKey(u => u.UserId)
      .IsRequired();

    builder.HasIndex(b => b.Username).IsUnique();
    builder.HasIndex(b => b.FullName);
  }
}
