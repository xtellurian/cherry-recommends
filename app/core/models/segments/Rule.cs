namespace SignalBox.Core
{
    public class Rule : NamedEntity
    {
        public Rule()
        {
        }
        public Rule(string name, long segmentId, string eventKey, string eventLogicalValue) : base(name)
        {
            SegmentId = segmentId;
            EventKey = eventKey;
            EventLogicalValue = eventLogicalValue;
        }

        public string EventKey { get; set; }
        public string EventLogicalValue { get; set; }
        public long SegmentId { get; set; }
    }
}