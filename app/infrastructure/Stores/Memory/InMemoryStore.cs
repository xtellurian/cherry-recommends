using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SignalBox.Infrastructure;

namespace SignalBox.Core
{
    public abstract class InMemoryStore<T> : IEntityStore<T> where T : Entity
    {
        protected Dictionary<long, T> store = new Dictionary<long, T>();

        public IStorageContext Context => new InMemoryStorageContext();

        public Task<int> Count(Expression<Func<T, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> Create(T entity)
        {
            entity.Id = store.Keys.Any() ? store.Keys.Max() + 1 : 1;
            store[entity.Id] = entity;
            return Task.FromResult(entity);
        }

        public Task<bool> Exists(long id)
        {
            return Task.FromResult(store.ContainsKey(id));
        }

        public Task<IEnumerable<T>> List(int n = 100)
        {
            return Task.FromResult(store.Values.ToList() as IEnumerable<T>);
        }

        public Task<Paginated<T>> Query(int page, Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Paginated<T>> Query<TProperty>(int page, Expression<Func<T, TProperty>> include, Expression<Func<T, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> Read(long id)
        {
            if (store.ContainsKey(id))
            {
                return Task.FromResult(store[id]);
            }
            else
            {
                throw new StorageException($"The Id {id} does not exist in store {(this)}");
            }
        }

        public Task<T> Read<TProperty>(long id, Expression<Func<T, TProperty>> include)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Remove(long id)
        {
            if (store.ContainsKey(id))
            {
                store.Remove(id);
                return Task.FromResult(store.ContainsKey(id));
            }
            else
            {
                return Task.FromResult(true);
            }
        }

        public Task<T> Update(T entity)
        {
            if (entity == null)
            {
                throw new System.NullReferenceException("Cannot save a null entity");
            }

            if (store.ContainsKey(entity.Id))
            {
                store[entity.Id] = entity;
                return Task.FromResult(entity);
            }
            else
            {
                throw new EntityNotFoundException<T>(entity.Id);
            }
        }
    }
}