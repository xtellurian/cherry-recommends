using System;
using System.Linq.Expressions;

namespace SignalBox.Core
{
#nullable enable
    public class EntityStoreIterateOptions<T> where T : Entity
    {
        private static readonly Expression<Func<T, bool>> defaultPredicate = _ => true;
        public EntityStoreIterateOptions(Expression<Func<T, bool>>? predicate = null, IterateOrderBy orderBy = IterateOrderBy.DescendingId)
        {
            Predicate = predicate ?? defaultPredicate;
            OrderBy = orderBy;
        }

        public Expression<Func<T, bool>> Predicate { get; set; } = defaultPredicate; // default to all true
        public IterateOrderBy OrderBy { get; set; }
        public ChangeTrackingOptions ChangeTracking { get; set; } = ChangeTrackingOptions.TrackAll;
    }
}
