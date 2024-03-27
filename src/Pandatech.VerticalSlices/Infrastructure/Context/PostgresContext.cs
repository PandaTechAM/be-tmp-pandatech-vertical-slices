﻿using Hangfire.EntityFrameworkCore;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using PandaTech.IEnumerableFilters.PostgresContext;
using Pandatech.VerticalSlices.Domain.Entities;

namespace Pandatech.VerticalSlices.Infrastructure.Context;

//hint for migration: dotnet ef migrations add --project src\Pandatech.VerticalSlices\Pandatech.VerticalSlices.csproj --configuration Debug --output-dir Infrastructure/Context/Migrations
public class PostgresContext(DbContextOptions<PostgresContext> options) : PostgresDbContext(options)
{
  public DbSet<UserTokenEntity> UserTokens { get; set; } = null!;
  public DbSet<UserEntity> Users { get; set; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.OnHangfireModelCreating();
    modelBuilder.AddTransactionalOutboxEntities();
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
  }
}
