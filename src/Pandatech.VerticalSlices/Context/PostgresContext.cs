using EFCore.AuditBase;
using GridifyExtensions.DbContextFunction;
using MassTransit.PostgresOutbox.Abstractions;
using MassTransit.PostgresOutbox.Entities;
using MassTransit.PostgresOutbox.Extensions;
using Microsoft.EntityFrameworkCore;
using Pandatech.VerticalSlices.Domain.Entities;

namespace Pandatech.VerticalSlices.Context;

//dotnet ef migrations add --project src\Pandatech.VerticalSlices\Pandatech.VerticalSlices.csproj --configuration Debug --output-dir Context/Migrations
public class PostgresContext : PostgresFunctions, IOutboxDbContext, IInboxDbContext
{
   public PostgresContext(DbContextOptions<PostgresContext> options) : base(options)
   {
      this.UseAuditPropertyValidation();
   }

   public DbSet<Token> Tokens { get; set; } = null!;
   public DbSet<User> Users { get; set; } = null!;
   public DbSet<UserConfig> UserConfigs { get; set; } = null!;

   public DbSet<InboxMessage> InboxMessages { get; set; }

   public DbSet<OutboxMessage> OutboxMessages { get; set; }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);
      modelBuilder.ConfigureInboxOutboxEntities();
      modelBuilder.FilterOutDeletedMarkedObjects();
      modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
   }
}
