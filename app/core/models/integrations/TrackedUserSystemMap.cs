using System.Text.Json.Serialization;

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
        public long IntegratedSystemId { get; protected set; }
        [JsonIgnore]
        public IntegratedSystem IntegratedSystem { get; set; }
        [JsonIgnore]
        public long TrackedUserId { get; protected set; }
        [JsonIgnore]
        public TrackedUser TrackedUser { get; set; }
    }
}