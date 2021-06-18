using System.Collections.Generic;

namespace SignalBox.Core
{
    public class TrackedUserTouchpoint : Entity
    {
        public TrackedUserTouchpoint()
        { }

        public TrackedUserTouchpoint(TrackedUser trackedUser, Touchpoint touchpoint, Dictionary<string, object> values, int version)
        {
            TrackedUser = trackedUser;
            Touchpoint = touchpoint;
            Values = values;
            Version = version;
        }

        public int Version { get; set; }
        public long TrackedUserId { get; set; }
        public TrackedUser TrackedUser { get; set; }
        public Touchpoint Touchpoint { get; set; }
        public Dictionary<string, object> Values { get; set; }
    }
}