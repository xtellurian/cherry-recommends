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
        protected const int PageSize = 100;
        public EFEntityStoreBase(SignalBoxDbContext context, Func<SignalBoxDbContext, DbSet<T>> selector)
        : base(context, selector)
        {
        }

        public async Task<int> Count(Expression<Func<T, bool>> predicate = null)
        {
            return await Set.CountAsync(predicate ?? ((x) => true));
        }

        public async Task<T> Create(T entity)
        {
            var result = await Set.AddAsync(entity);
            return result.Entity;
        }

        public async Task<bool> Exists(long id)
        {
            return await Set.AnyAsync(_ => _.Id == id);
        }

        public async Task<Paginated<T>> Query(int page, Expression<Func<T, bool>> predicate = null)
        {
            predicate ??= _ => true; // default to all entities
            var itemCount = await Set.CountAsync(predicate);
            List<T> results;

            if (itemCount > 0) // check and let's see whether the query is worth running against the database
            {
                results = await Set
                    .Where(predicate)
                    .OrderByDescending(_ => _.LastUpdated)
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
                return await Set.SingleAsync(_ => _.Id == id);
            }
            catch (Exception ex)
            {
                throw new StorageException($"An exception was thrown when finding type: s${typeof(T)} with Id:${id}", ex);
            }
        }

        public async Task<bool> Remove(long id)
        {
            var entity = await Set.SingleAsync(_ => _.Id == id);
            var result = Set.Remove(entity);
            return result.State.HasFlag(EntityState.Deleted);
        }

        public async Task<T> Update(T entity)
        {
            entity = await Set.SingleAsync(_ => _.Id == entity.Id);
            return entity;
        }
    }
}