namespace SignalBox.Core
{
    public class TrackedUserSystemMap : Entity
    {
        public TrackedUserSystemMap()
        {
        }

        public TrackedUserSystemMap(string userId, IntegratedSystem integratedSystem, TrackedUser trackedUser)
        {
            UserId = userId;
            IntegratedSystem = integratedSystem;
            TrackedUser = trackedUser;
        }

        public string UserId { get; set; }
        public IntegratedSystem IntegratedSystem { get; set; }
        public TrackedUser TrackedUser { get; set; }
    }
}