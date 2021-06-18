using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface IStore<T> where T : class
    {
        Task<bool> Exists(long id);
        Task<T> Create(T entity);
        Task<T> Read(long id);
        Task<T> Read<TProperty>(long id, Expression<Func<T, TProperty>> include);
        Task<T> Update(T entity);
        Task<int> Count(Expression<Func<T, bool>> predicate = null);
        Task<Paginated<T>> Query(int page, Expression<Func<T, bool>> predicate = null);
        Task<Paginated<T>> Query<TProperty>(int page, Expression<Func<T, TProperty>> include, Expression<Func<T, bool>> predicate = null);
        Task<bool> Remove(long id);
    }
}