namespace SignalBox.Core
{
    public enum ChangeTrackingOptions
    {
        TrackAll,
        NoTracking,
        NoTrackingWithIdentityResolution
    }

    public class EntityStoreReadOptions
    {
        public ChangeTrackingOptions ChangeTracking { get; set; } = ChangeTrackingOptions.TrackAll;
    }
}