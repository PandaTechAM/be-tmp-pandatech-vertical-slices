using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PandaTech.IEnumerableFilters;
using PandaTech.IEnumerableFilters.Dto;
using PandaTech.IEnumerableFilters.Extensions;
using Pandatech.VerticalSlices.Infrastructure.Context;
using ResponseCrafter.StandardHttpExceptions;

namespace Pandatech.VerticalSlices.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity>(PostgresContext context) : DatabaseOperationsBase(context)
   where TEntity : class
{
   public async Task<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
   {
      return await Context.Set<TEntity>().FindAsync([id], cancellationToken: cancellationToken);
   }

   public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
   {
      return await Context.Set<TEntity>().FindAsync([id], cancellationToken: cancellationToken);
   }

   public async Task<TEntity?> GetByIdNoTrackingAsync(long id, CancellationToken cancellationToken = default)
   {
      return await Context.Set<TEntity>().AsNoTracking()
         .FirstOrDefaultAsync(e => EF.Property<object>(e, "Id").Equals(id), cancellationToken: cancellationToken);
   }

   public async Task<TEntity?> GetByIdNoTrackingAsync(Guid id, CancellationToken cancellationToken = default)
   {
      return await Context.Set<TEntity>().AsNoTracking()
         .FirstOrDefaultAsync(e => EF.Property<object>(e, "Id").Equals(id), cancellationToken: cancellationToken);
   }

   public IQueryable<TEntity> GetAll(CancellationToken cancellationToken = default)
   {
      return Context.Set<TEntity>().AsQueryable();
   }

   public IQueryable<TEntity> GetAllNoTracking(CancellationToken cancellationToken = default)
   {
      return Context.Set<TEntity>().AsNoTracking();
   }

   public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate,
      CancellationToken cancellationToken = default)
   {
      return Context.Set<TEntity>().Where(predicate);
   }

   public IQueryable<TEntity> FindNoTracking(Expression<Func<TEntity, bool>> predicate,
      CancellationToken cancellationToken = default)
   {
      return Context.Set<TEntity>().Where(predicate).AsNoTracking();
   }

   public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
   {
      await Context.Set<TEntity>().AddAsync(entity, cancellationToken);
   }

   public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
   {
      await Context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
   }

   public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
   {
      return await Context.Set<TEntity>().AnyAsync(predicate);
   }

   public void Update(TEntity entity, CancellationToken cancellationToken = default)
   {
      Context.Set<TEntity>().Attach(entity);
      Context.Entry(entity).State = EntityState.Modified;
   }

   public void Remove(TEntity entity, CancellationToken cancellationToken = default)
   {
      Context.Set<TEntity>().Remove(entity);
   }

   public void RemoveRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
   {
      Context.Set<TEntity>().RemoveRange(entities);
   }

   public async Task<DistinctColumnValuesResult> GetColumnValuesAsync(string columnName, int page,
      int pageSize, string dataRequest)
   {
      try
      {
         return await Context.Set<TEntity>()
            .DistinctColumnValuesAsync(
               GetDataRequest.FromString(dataRequest).Filters,
               columnName,
               pageSize,
               page);
      }
      catch (Exception ex)
      {
         throw new BadRequestException(ex.Message);
      }
   }

   public Task<List<FilterInfo>> GetFiltersAsync()
   {
      try
      {
         var tableName = $"{typeof(TEntity).Name}FilterModel";
         return Task.FromResult(FilterExtenders.GetFilters(typeof(Program).Assembly, tableName));
      }
      catch (Exception ex)
      {
         throw new BadRequestException(ex.Message);
      }
   }
}
