namespace SignalBox.Core
{
    /// <summary>
    /// A factory implementation that allows the caller to get any registered store.
    /// </summary>
    public interface IStoreCollection
    {
        /// <summary>
        /// Get a store registered for an Entity
        /// </summary>
        /// <typeparam name="TStore">The type of store</typeparam>
        /// <typeparam name="TEntity">The entity being stored.</typeparam>
        /// <returns>Throws if store is not registered</returns> 
        TStore ResolveStore<TStore, TEntity>() where TStore : class, IStore<TEntity> where TEntity : class;
    }
}
