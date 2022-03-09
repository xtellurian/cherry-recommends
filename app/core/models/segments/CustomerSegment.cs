using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class CustomerSegment
    {
        public long SegmentId { get; set; }
        public long CustomerId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Segment Segment { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Customer Customer { get; set; }

        protected CustomerSegment()
        { }

        public CustomerSegment(long customerId, long segmentId)
        {
            this.CustomerId = customerId;
            this.SegmentId = segmentId;
        }

        public CustomerSegment(Customer customer, Segment segment)
        {
            this.CustomerId = customer.Id;
            this.SegmentId = segment.Id;
            this.Customer = customer;
            this.Segment = segment;
        }
    }
}