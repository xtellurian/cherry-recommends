using System;
using System.Linq.Expressions;

namespace SignalBox.Core
{
#nullable enable
    public class EntityStoreQueryOptions<TEntity> where TEntity : Entity
    {
        public EntityStoreQueryOptions()
        { }

        public EntityStoreQueryOptions(IPaginate paginate, Expression<Func<TEntity, bool>>? predicate = null)
        {
            Page = paginate.Page >= 1 ? paginate.Page : 1; // Minimum is 1
            PageSize = paginate.PageSize;
            Predicate = predicate ??= _ => true;
        }
        public EntityStoreQueryOptions(int page = 1, Expression<Func<TEntity, bool>>? predicate = null)
        {
            Page = page;
            Predicate = predicate ??= _ => true;
        }

        /// <summary>
        /// Page to use. Will always be >= 1
        /// </summary>
        public int Page { get; } = 1;
        public int? PageSize { get; set; }
        public Expression<Func<TEntity, bool>> Predicate { get; } = _ => true;
        /// <summary>
        /// Will add a filter predicate that matches similar strings with common fields.
        /// Doesn't apply to the base Entity, because it has no text fields.
        /// </summary>
        public string? SearchTerm { get; set; }
        /// <summary>
        /// For EF Stores, controls change tracking for the returned entities.
        /// </summary>
        public ChangeTrackingOptions ChangeTracking { get; set; } = ChangeTrackingOptions.TrackAll;
    }
}