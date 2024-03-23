using Microsoft.EntityFrameworkCore.Storage;
using Pandatech.VerticalSlices.Infrastructure.Context;

namespace Pandatech.VerticalSlices.Infrastructure.Repositories;

public abstract class DatabaseOperationsBase(PostgresContext context)
{
   protected PostgresContext Context { get; } = context;

   
   public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
   {
      return await Context.Database.BeginTransactionAsync(cancellationToken);
   }

   public async Task CommitAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
   {
      try
      {
         await Context.SaveChangesAsync(cancellationToken);
         await transaction.CommitAsync(cancellationToken);
      }
      catch
      {
         await transaction.RollbackAsync(cancellationToken);
         throw;
      }
   }

   public async Task RollbackAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
   {
      await transaction.RollbackAsync(cancellationToken);
   }

   public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
   {
      await Context.SaveChangesAsync(cancellationToken);
   }
}
