using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pandatech.VerticalSlices.Domain.Entities;

namespace Pandatech.VerticalSlices.Context.EntityConfigurations;

public class UserTokenEntityConfiguration : IEntityTypeConfiguration<Token>
{
   public void Configure(EntityTypeBuilder<Token> builder)
   {
      builder.HasKey(x => x.Id);


      builder.HasOne(x => x.User)
         .WithMany(u => u.Tokens)
         .HasForeignKey(x => x.UserId)
         .IsRequired();

      builder.HasOne(x => x.PreviousToken)
         .WithOne()
         .HasForeignKey<Token>(x => x.PreviousUserTokenId)
         .IsRequired(false);

      builder.HasIndex(x => x.AccessTokenHash).IsUnique();
      builder.HasIndex(x => x.RefreshTokenHash).IsUnique();
   }
}
