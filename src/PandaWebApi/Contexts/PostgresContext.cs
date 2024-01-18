using Hangfire.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PandaTech.IEnumerableFilters.PostgresContext;
using PandaWebApi.Models;

namespace PandaWebApi.Contexts;

//hint for migration: dotnet ef migrations add v1
public class PostgresContext(DbContextOptions<PostgresContext> options) : PostgresDbContext(options)
{
    public DbSet<UserToken> UserTokens { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserAuthenticationHistory> UserAuthenticationHistory { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<HangfireCounter>(entity => entity.ToTable("hangfire_counter", "hangfire"));
        modelBuilder.Entity<HangfireHash>(entity => entity.ToTable("hangfire_hash", "hangfire"));
        modelBuilder.Entity<HangfireJob>(entity => entity.ToTable("hangfire_job", "hangfire"));
        modelBuilder.Entity<HangfireJobParameter>(entity => entity.ToTable("hangfire_job_parameter", "hangfire"));
        modelBuilder.Entity<HangfireList>(entity => entity.ToTable("hangfire_list", "hangfire"));
        modelBuilder.Entity<HangfireQueuedJob>(entity => entity.ToTable("hangfire_queued_job", "hangfire"));
        modelBuilder.Entity<HangfireQueuedJob>(entity => entity.ToTable("hangfire_queued_job", "hangfire"));
        modelBuilder.Entity<HangfireServer>(entity => entity.ToTable("hangfire_server", "hangfire"));
        modelBuilder.Entity<HangfireSet>(entity => entity.ToTable("hangfire_set", "hangfire"));
        modelBuilder.Entity<HangfireState>(entity => entity.ToTable("hangfire_state", "hangfire"));
        modelBuilder.Entity<HangfireLock>(entity => entity.ToTable("hangfire_lock", "hangfire"));

        modelBuilder.OnHangfireModelCreating();
    }
}
public class PostgresContextFactory : IDesignTimeDbContextFactory<PostgresContext>
{
    public PostgresContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PostgresContext>();

        optionsBuilder.UseNpgsql().UseSnakeCaseNamingConvention();

        return new PostgresContext(optionsBuilder.Options);
    }
}