using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SignalBox.Core
{
#nullable enable
    public interface IEntityStore<T> : IStore<T> where T : Entity
    {
        IStorageContext Context { get; }
        Task<Paginated<T>> Query(EntityStoreQueryOptions<T>? queryOptions = null);
        Task<Paginated<T>> Query<TProperty>(Expression<Func<T, TProperty>> include, EntityStoreQueryOptions<T>? queryOptions = null);
        IAsyncEnumerable<T> Iterate(EntityStoreIterateOptions<T>? options = null);
    }
}