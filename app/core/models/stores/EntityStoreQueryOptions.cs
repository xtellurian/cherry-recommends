using System;
using System.Linq.Expressions;

namespace SignalBox.Core
{
#nullable enable
    public class EntityStoreQueryOptions<TEntity> where TEntity : Entity
    {
        public EntityStoreQueryOptions()
        { }

        public EntityStoreQueryOptions(int page = 1, Expression<Func<TEntity, bool>>? predicate = null)
        {
            Page = page;
            Predicate = predicate ??= _ => true;
        }

        public int Page { get; } = 1;
        public Expression<Func<TEntity, bool>> Predicate { get; } = _ => true;
    }
}