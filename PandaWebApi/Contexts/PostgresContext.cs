using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PandaWebApi.Models;

namespace PandaWebApi.Contexts;

public class PostgresContext : IdentityDbContext<User>
{
    public PostgresContext(DbContextOptions<PostgresContext> options) : base(options)
    {
    }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(b =>
        {
            b.ToTable("users");
        });

        modelBuilder.Entity<IdentityUserClaim<string>>(b =>
        {
            b.ToTable("user_claims");
        });

        modelBuilder.Entity<IdentityUserLogin<string>>(b =>
        {
            b.ToTable("user_logins");
        });

        modelBuilder.Entity<IdentityUserToken<string>>(b =>
        {
            b.ToTable("user_tokens");
        });

        modelBuilder.Entity<IdentityRole>(b =>
        {
            b.ToTable("roles");
        });

        modelBuilder.Entity<IdentityRoleClaim<string>>(b =>
        {
            b.ToTable("role_claims");
        });

        modelBuilder.Entity<IdentityUserRole<string>>(b =>
        {
            b.ToTable("user_roles");
        });
    }
}


public class PostgresContextFactory : IDesignTimeDbContextFactory<PostgresContext>{
    public PostgresContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PostgresContext>();
        
        optionsBuilder.UseNpgsql().UseSnakeCaseNamingConvention();
        
        return new PostgresContext(optionsBuilder.Options);
    }
}

//dotnet ef migrations add v1