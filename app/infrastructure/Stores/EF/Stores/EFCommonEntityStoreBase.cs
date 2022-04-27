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
    public abstract class EFCommonEntityStoreBase<T> : EFEnvironmentScopedEntityStoreBase<T>, ICommonEntityStore<T> where T : CommonEntity
    {
        public EFCommonEntityStoreBase(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentProvider environmentService, Func<SignalBoxDbContext, DbSet<T>> selector)
        : base(contextProvider, environmentService, selector)
        { }

        public override async Task<T> Create(T entity)
        {
            // use QuerySet to allow duplicate common ID in different environments.
            if (await QuerySet.AnyAsync(_ => _.CommonId == entity.CommonId))
            {
                throw new StorageException($"CommonId {entity.CommonId} of type {typeof(T).Name} already exists in the database.");
            }
            else
                return await base.Create(entity);
        }

        public override async Task<Paginated<T>> Query(EntityStoreQueryOptions<T> queryOptions = null)
        {
            queryOptions ??= new EntityStoreQueryOptions<T>();
            var pageSize = queryOptions.PageSize ?? DefaultPageSize;
            var predicateBuilder = PredicateBuilder.New<T>(true);
            if (!string.IsNullOrEmpty(queryOptions.SearchTerm))
            {
                predicateBuilder = predicateBuilder.And(_ =>
                    EF.Functions.Like(_.CommonId, $"%{queryOptions.SearchTerm}%") ||
                    EF.Functions.Like(_.Name, $"%{queryOptions.SearchTerm}%"));
            }
            if (queryOptions.Predicate != null)
            {
                predicateBuilder = predicateBuilder.And(queryOptions.Predicate);
            }

            var itemCount = await QuerySet.CountAsync(predicateBuilder);
            List<T> results;

            if (itemCount > 0) // check and let's see whether the query is worth running against the database
            {
                results = await QuerySet
                    .Where(predicateBuilder)
                    .OrderByDescending(_ => _.LastUpdated)
                    .Skip((queryOptions.Page - 1) * pageSize).Take(pageSize)
                    .ToListAsync();
            }
            else
            {
                results = new List<T>();
            }
            var pageCount = (int)Math.Ceiling((double)itemCount / pageSize);
            return new Paginated<T>(results, pageCount, itemCount, queryOptions.Page);
        }

        public virtual async Task<T> ReadFromCommonId<TProperty>(string commonId, Expression<Func<T, TProperty>> include)
        {
            try
            {
                return await QuerySet
                    .Include(include)
                    .FirstAsync(_ => _.CommonId == commonId);
            }
            catch (Exception ex)
            {
                throw new EntityNotFoundException(typeof(T), commonId, ex);
            }
        }

        public async Task<bool> ExistsFromCommonId(string commonId)
        {
            return await QuerySet
                .AnyAsync(_ => _.CommonId == commonId);
        }

        public async Task<bool> ExistsFromCommonId(string commonId, long? environmentId)
        {
            return await Set
                 .AnyAsync(_ => _.EnvironmentId == environmentId && _.CommonId == commonId);
        }

        public virtual async Task<T> ReadFromCommonId(string commonId)
        {
            try
            {
                return await QuerySet
                    .FirstAsync(_ => _.CommonId == commonId);
            }
            catch (Exception ex)
            {
                throw new EntityNotFoundException(typeof(T), commonId, ex);
            }
        }
        public async Task<T> ReadFromCommonId<TProperty>(string commonId, long? environmentId, Expression<Func<T, TProperty>> include)
        {
            return await Set
                .Include(include)
                .FirstAsync(_ => _.EnvironmentId == environmentId && _.CommonId == commonId);
        }

    }
}