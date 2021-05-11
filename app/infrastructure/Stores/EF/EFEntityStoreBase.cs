using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure
{
    public class EFEntityStoreBase<T> : EFStoreBase<T>, IEntityStore<T> where T : Entity
    {
        public EFEntityStoreBase(SignalBoxDbContext context, Func<SignalBoxDbContext, DbSet<T>> selector)
        : base(context, selector)
        {
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

        public async Task<IEnumerable<T>> List()
        {
            return await Set.ToListAsync();
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