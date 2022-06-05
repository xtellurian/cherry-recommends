namespace SignalBox.Core
{
    /// <summary>
    /// Controls how the query will be handled by the store.
    /// </summary>
    public class EntityStoreReadOptions
    {
        /// <summary>
        /// For EF stores, controls change tracking for the returned entity
        /// </summary>
        public ChangeTrackingOptions ChangeTracking { get; set; } = ChangeTrackingOptions.TrackAll;
    }
}
