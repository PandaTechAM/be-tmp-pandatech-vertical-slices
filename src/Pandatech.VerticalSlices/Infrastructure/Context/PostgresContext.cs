using EFCore.AuditBase;
using Hangfire.EntityFrameworkCore;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using PandaTech.IEnumerableFilters.PostgresContext;
using Pandatech.VerticalSlices.Domain.Entities;


namespace Pandatech.VerticalSlices.Infrastructure.Context;

public class PostgresContext : PostgresDbContext
{
   public PostgresContext(DbContextOptions<PostgresContext> options) : base(options)
   {
      this.UseAuditPropertyValidation();
   }

   public DbSet<UserTokenEntity> UserTokens { get; set; } = null!;
   public DbSet<UserEntity> Users { get; set; } = null!;

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);

      modelBuilder.OnHangfireModelCreating();
      modelBuilder.AddTransactionalOutboxEntities();
      modelBuilder.FilterOutDeletedMarkedObjects();
      modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
   }
}
