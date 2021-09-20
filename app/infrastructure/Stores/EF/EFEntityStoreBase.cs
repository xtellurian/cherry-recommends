using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFEntityStoreBase<T> : EFStoreBase<T>, IEntityStore<T> where T : Entity
    {
        protected virtual Expression<Func<T, DateTimeOffset>> defaultOrderBy => _ => _.LastUpdated;
        protected virtual int PageSize => 100;
        public EFEntityStoreBase(SignalBoxDbContext context, Func<SignalBoxDbContext, DbSet<T>> selector)
        : base(context, selector)
        {
        }

        public async Task<int> Count(Expression<Func<T, bool>> predicate = null)
        {
            if (await QuerySet.AnyAsync(predicate ?? ((x) => true)))
            {
                return await QuerySet
                    .CountAsync(predicate ?? ((x) => true));
            }
            else
            {
                return 0;
            }
        }

        public async Task<TResult> Min<TResult>(Expression<Func<T, TResult>> selector)
        {
            return await QuerySet.DefaultIfEmpty(null).MinAsync(selector);
        }

        public async Task<TResult> Min<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
        {
            if (await QuerySet.Where(predicate).Select(selector).AnyAsync(_ => _ != null))
            {
                return await QuerySet.Where(predicate).MinAsync(selector);
            }
            else
            {
                throw new InvalidStorageAccessException($"The entities of type {typeof(T)} have to minimum value for this query");
            }
        }

        public async Task<TResult> Max<TResult>(Expression<Func<T, TResult>> selector)
        {
            return await QuerySet.MaxAsync(selector);
        }

        public virtual async Task<T> Create(T entity)
        {
            var result = await Set.AddAsync(entity);
            return result.Entity;
        }

        public async Task<bool> Exists(long id)
        {
            return await QuerySet.AnyAsync(_ => _.Id == id);
        }

        public async Task<Paginated<T>> Query(int page, Expression<Func<T, bool>> predicate = null)
        {
            predicate ??= _ => true; // default to all entities
            var itemCount = await QuerySet.CountAsync(predicate);
            List<T> results;

            if (itemCount > 0) // check and let's see whether the query is worth running against the database
            {
                results = await QuerySet
                    .Where(predicate)
                    .OrderByDescending(defaultOrderBy)
                    .Skip((page - 1) * PageSize).Take(PageSize)
                    .ToListAsync();
            }
            else
            {
                results = new List<T>();
            }
            var pageCount = (int)Math.Ceiling((double)itemCount / PageSize);
            return new Paginated<T>(results, pageCount, itemCount, page);
        }

        public async Task<Paginated<T>> Query<TProperty>(int page,
                                                         Expression<Func<T, TProperty>> include,
                                                         Expression<Func<T, bool>> predicate = null)
        {
            predicate ??= _ => true; // default to all entities
            var itemCount = await QuerySet.CountAsync(predicate);
            List<T> results;

            if (itemCount > 0) // check and let's see whether the query is worth running against the database
            {
                results = await QuerySet
                    .Where(predicate)
                    .Include(include)
                    .OrderByDescending(defaultOrderBy)
                    .Skip((page - 1) * PageSize).Take(PageSize)
                    .ToListAsync();
            }
            else
            {
                results = new List<T>();
            }
            var pageCount = (int)Math.Ceiling((double)itemCount / PageSize);
            return new Paginated<T>(results, pageCount, itemCount, page);
        }

        public virtual async Task<T> Read(long id)
        {
            try
            {
                return await QuerySet.SingleAsync(_ => _.Id == id);
            }
            catch (Exception ex)
            {
                throw new EntityNotFoundException(typeof(T), id, ex);
            }
        }
        public virtual async Task<T> Read<TProperty>(long id, Expression<Func<T, TProperty>> include)
        {
            try
            {
                return await QuerySet.Include(include).SingleAsync(_ => _.Id == id);
            }
            catch (Exception ex)
            {
                throw new EntityNotFoundException(typeof(T), id, ex);
            }
        }

        public async Task<bool> Remove(long id)
        {
            try
            {
                var entity = await QuerySet.SingleAsync(_ => _.Id == id);
                var result = Set.Remove(entity);
                return result.State.HasFlag(EntityState.Deleted);
            }
            catch (Exception ex)
            {
                throw new StorageException($"Failed to delete entity {id} of type {typeof(T).Name}", ex);
            }
        }

        public async Task<T> Update(T entity)
        {
            entity = await QuerySet.SingleAsync(_ => _.Id == entity.Id);
            return entity;
        }

        public async Task LoadMany<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> propertyExpression) where TProperty : class
        {
            await context.Entry(entity)
                .Collection(propertyExpression)
                .LoadAsync();
        }

        public async Task Load<TProperty>(T entity, Expression<Func<T, TProperty>> propertyExpression) where TProperty : class
        {
            await context.Entry(entity)
                .Reference(propertyExpression)
                .LoadAsync();
        }
    }
}