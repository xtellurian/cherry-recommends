using System.Threading.Tasks;

namespace SignalBox.Core
{
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
    }
}