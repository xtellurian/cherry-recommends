using System.Collections.Generic;

namespace SignalBox.Core
{
    public class TrackedUserTouchpoint : Entity
    {
        protected TrackedUserTouchpoint()
        { }

        public TrackedUserTouchpoint(Customer customer, Touchpoint touchpoint, Dictionary<string, object> values, int version)
        {
            Customer = customer;
            Touchpoint = touchpoint;
            Values = values;
            Version = version;
        }

        public int Version { get; set; }
        public long TrackedUserId { get; set; }
        public Customer TrackedUser => Customer;
        public Customer Customer { get; set; }
        public long TouchpointId { get; set; }
        public Touchpoint Touchpoint { get; set; }
        public Dictionary<string, object> Values { get; set; }
    }
}