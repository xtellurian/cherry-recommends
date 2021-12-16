namespace SignalBox.Web.Dto
{
    public class CreateRuleDto : DtoBase
    {
        public string Name { get; set; }
        public long SegmentId { get; set; }
        public string EventKey { get; set; }
        public string EventLogicalValue { get; set; }
    }
}