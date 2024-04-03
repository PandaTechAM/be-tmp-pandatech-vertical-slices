using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pandatech.VerticalSlices.Domain.Entities;

namespace Pandatech.VerticalSlices.Infrastructure.Context.EntityConfigurations;

public class UserTokenEntityConfiguration : IEntityTypeConfiguration<UserTokenEntity>
{
  public void Configure(EntityTypeBuilder<UserTokenEntity> builder)
  {
    builder.HasKey(x => x.Id);

    // builder.Property(x => x.UserId).IsRequired();
    // builder.Property(x => x.PreviousUserTokenId);
    // builder.Property(x => x.AccessTokenHash).IsRequired();
    // builder.Property(x => x.RefreshTokenHash).IsRequired();
    // builder.Property(x => x.AccessTokenExpiresAt).IsRequired();
    // builder.Property(x => x.RefreshTokenExpiresAt).IsRequired();
    // builder.Property(x => x.InitialRefreshTokenCreatedAt).IsRequired();
    // builder.Property(x => x.CreatedAt).IsRequired();
    // builder.Property(x => x.UpdatedAt).IsRequired();

    builder.HasOne(x => x.User)
      .WithMany(u => u.UserTokens)
      .HasForeignKey(x => x.UserId)
      .IsRequired();
    
    builder.HasOne(x => x.PreviousUserTokenEntity)
      .WithOne()
      .HasForeignKey<UserTokenEntity>(x => x.PreviousUserTokenId)
      .IsRequired(false);

    builder.HasIndex(x => x.AccessTokenHash).IsUnique();
    builder.HasIndex(x => x.RefreshTokenHash).IsUnique();
  }
}
