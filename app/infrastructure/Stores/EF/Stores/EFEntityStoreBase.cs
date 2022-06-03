using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
#nullable enable
    public abstract class EFEntityStoreBase<T> : EFStoreBase<T>, IEntityStore<T> where T : Entity
    {
        protected virtual Expression<Func<T, DateTimeOffset>> defaultOrderBy => _ => _.LastUpdated;
        protected virtual int DefaultPageSize => 100;
        protected EFEntityStoreBase(IDbContextProvider<SignalBoxDbContext> contextProvider, Func<SignalBoxDbContext, DbSet<T>> selector)
        : base(contextProvider, selector)
        { }

        protected QueryTrackingBehavior ToEntityFramework(ChangeTrackingOptions? changeTrackingOptions)
        {
            return changeTrackingOptions switch
            {
                ChangeTrackingOptions.TrackAll => QueryTrackingBehavior.TrackAll,
                ChangeTrackingOptions.NoTrackingWithIdentityResolution => QueryTrackingBehavior.NoTrackingWithIdentityResolution,
                ChangeTrackingOptions.NoTracking => QueryTrackingBehavior.NoTracking,
                _ => QueryTrackingBehavior.TrackAll,
            };
        }

        public async Task<int> Count(Expression<Func<T, bool>>? predicate = null)
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

        public async Task<TResult?> Min<TResult>(Expression<Func<T, TResult>> selector)
        {
            if (await QuerySet.AnyAsync())
            {
                return await QuerySet.MinAsync(selector);
            }
            else
            {
                return default;
            }
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
            if (context.Database.IsSqlite())
            {
                entity.Created = System.DateTimeOffset.Now;
            }
            var result = await Set.AddAsync(entity);
            return result.Entity;
        }

        public async Task<bool> Exists(long id)
        {
            return await QuerySet.AnyAsync(_ => _.Id == id);
        }

        public async Task<Paginated<T>> Query<TProperty>(Expression<Func<T, TProperty>>? include = null, EntityStoreQueryOptions<T>? queryOptions = null)
        {
            var predicateBuilder = PredicateBuilder.New<T>(true);
            queryOptions ??= new EntityStoreQueryOptions<T>();
            if (queryOptions.Predicate != null)
            {
                predicateBuilder = predicateBuilder.And(queryOptions.Predicate);
            }

            var itemCount = await QuerySet.CountAsync(predicateBuilder);
            List<T> results;
            var pageSize = queryOptions.PageSize ?? DefaultPageSize;
            if (itemCount > 0) // check and let's see whether the query is worth running against the database
            {
                var q = QuerySet
                    .Where(predicateBuilder)
                    .OrderByDescending(defaultOrderBy)
                    .Skip((queryOptions.Page - 1) * pageSize)
                    .Take(pageSize);

                if (include != null)
                {
                    q = q.Include(include);
                }

                results = await q.ToListAsync(); // manifest the query
            }
            else
            {
                results = new List<T>();
            }
            var pageCount = (int)Math.Ceiling((double)itemCount / pageSize);
            return new Paginated<T>(results, pageCount, itemCount, queryOptions.Page);
        }
        public async virtual Task<Paginated<T>> Query(EntityStoreQueryOptions<T>? queryOptions = null)
        {
            return await this.Query<object>(null, queryOptions);
        }

        public virtual async Task<T> Read(long id, EntityStoreReadOptions? options = null)
        {
            try
            {
                return await QuerySet
                    .AsTracking(ToEntityFramework(options?.ChangeTracking))
                    .SingleAsync(_ => _.Id == id);
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
            if (context.Database.IsSqlite())
            {
                entity.LastUpdated = System.DateTimeOffset.Now;
            }
            context.Entry(entity).State = EntityState.Modified;
            entity = await QuerySet.SingleAsync(_ => _.Id == entity.Id);
            return entity;
        }

        public async Task LoadMany<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>?>> propertyExpression) where TProperty : class
        {
            await context.Entry(entity)
                .Collection(propertyExpression)
                .LoadAsync();
        }

        public async Task Load<TProperty>(T entity, Expression<Func<T, TProperty?>> propertyExpression) where TProperty : class
        {
            await context.Entry(entity)
                .Reference(propertyExpression)
                .LoadAsync();
        }

        public virtual async IAsyncEnumerable<T> Iterate(Expression<Func<T, bool>>? predicate = null, IterateOrderBy orderBy = IterateOrderBy.DescendingId)
        {
            predicate ??= _ => true;
            bool hasMoreItems = await QuerySet.AnyAsync(predicate);
            if (!hasMoreItems)
            {
                yield break;
            }
            var maxId = await QuerySet.MaxAsync(_ => _.Id);
            var currentId = maxId + 1; // we query for ids less than this.
            while (hasMoreItems)
            {
                List<T> results = await RunQueryWithOrderby(predicate, currentId, orderBy);

                if (results.Any())
                {
                    currentId = results.Min(_ => _.Id); // get the smallest in the result

                    foreach (var item in results)
                    {
                        yield return item;
                    }

                    hasMoreItems = await QuerySet.AnyAsync(_ => _.Id < currentId);
                }
                else
                {
                    // break out of the iteration. no more results.
                    hasMoreItems = false;
                }
            }
        }

        private async Task<List<T>> RunQueryWithOrderby(Expression<Func<T, bool>> predicate, long currentId, IterateOrderBy orderBy)
        {

            return orderBy switch
            {
                IterateOrderBy.AscendingId =>
                    await QuerySet
                        .Where(predicate)
                        .Where(_ => _.Id < currentId)
                        .OrderBy(_ => _.Id) // ascending here
                        .Take(DefaultPageSize) // use the default page size when running an iteration
                        .ToListAsync(),
                _ =>
                    await QuerySet
                        .Where(predicate)
                        .Where(_ => _.Id < currentId)
                        .OrderByDescending(_ => _.Id) // descending here
                        .Take(DefaultPageSize) // use the default page size when running an iteration
                        .ToListAsync(),
            };
        }
    }
}