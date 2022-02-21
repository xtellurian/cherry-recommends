using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IEntityStore<T> : IStore<T> where T : Entity
    {
        IStorageContext Context { get; }
        Task<Paginated<T>> Query(EntityStoreQueryOptions<T> queryOptions = null);
        Task<Paginated<T>> Query<TProperty>(Expression<Func<T, TProperty>> include, EntityStoreQueryOptions<T> queryOptions = null);
    }
}