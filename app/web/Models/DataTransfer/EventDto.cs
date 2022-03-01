using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SignalBox.Core;

namespace SignalBox.Web.Dto
{
    public class EventDto : DtoBase
    {
        public string GetCustomerId() => CustomerId ?? CommonUserId;
        public string CommonUserId { get; set; }
        public string CustomerId { get; set; }
        [Required]
        public string EventId { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public long? RecommendationCorrelatorId { get; set; }
        public long? SourceSystemId { get; set; }
        public EventKinds Kind { get; set; }
        [Required]
        public string EventType { get; set; }
        public Dictionary<string, object> Properties { get; set; }
    }
}