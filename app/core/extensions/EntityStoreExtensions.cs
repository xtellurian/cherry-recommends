using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SignalBox.Core
{
#nullable enable
    public static class EntityStoreExtensions
    {
        /// <summary>
        /// Gets a common entity using either the internal or common id.
        /// </summary>
        public static async Task<T> GetEntity<T>(this ICommonEntityStore<T> store, string id, bool? useInternalId = null) where T : CommonEntity
        {
            if ((useInternalId == null || useInternalId == true) && long.TryParse(id, out var internalId))
            {
                return await store.Read(internalId);
            }
            else if (useInternalId == true)
            {
                // the middle case, where useInternalId was set to true, but the id was not a long.
                throw new BadRequestException("Internal Ids must be long integers");
            }
            else
            {
                return await store.ReadFromCommonId(id);
            }
        }

        public static Customer ToCoreRepresentation(this PendingCustomer pending)
        {
            return new Customer(pending.CommonId, pending.Name, pending.Properties)
            {
                Email = pending.Email,
                EnvironmentId = pending.EnvironmentId,
            };
        }

        public static async Task<Paginated<T>> Query<T>(this IEntityStore<T> store,
                                                        IPaginate paginate,
                                                        Expression<Func<T, bool>>? predicate = null) where T : Entity
        {
            return await store.Query(new EntityStoreQueryOptions<T>(paginate, predicate));
        }

        public static async Task<Paginated<T>> NoTrackingQuery<T>(this IEntityStore<T> store,
                                                        IPaginate paginate,
                                                        string? searchTerm = null,
                                                        Expression<Func<T, bool>>? predicate = null) where T : Entity
        {
            return await store.Query(new EntityStoreQueryOptions<T>(paginate, predicate)
            {
                ChangeTracking = ChangeTrackingOptions.NoTracking,
                SearchTerm = searchTerm
            });
        }

        public static async Task<Paginated<T>> Query<T, TProperty>(this IEntityStore<T> store,
                                                                   IPaginate paginate,
                                                                   Expression<Func<T, TProperty>> include,
                                                                   Expression<Func<T, bool>>? predicate = null) where T : Entity
        {
            return await store.Query<TProperty>(include, new EntityStoreQueryOptions<T>(paginate, predicate));
        }
    }
}