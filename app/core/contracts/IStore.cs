using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

#nullable enable
namespace SignalBox.Core
{
    public interface IStore<T> where T : class
    {
        Task<bool> Exists(long id);
        Task<T> Create(T entity);
        Task<T> Read(long id, EntityStoreReadOptions? options = null);
        Task<T> Read<TProperty>(long id, Expression<Func<T, TProperty>> include);
        Task<T> Update(T entity);
        Task<int> Count(Expression<Func<T, bool>>? predicate = null);
        Task<bool> Remove(long id);
        Task LoadMany<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>?>> propertyExpression) where TProperty : class;
        Task Load<TProperty>(T entity, Expression<Func<T, TProperty?>> propertyExpression) where TProperty : class;
    }
}
