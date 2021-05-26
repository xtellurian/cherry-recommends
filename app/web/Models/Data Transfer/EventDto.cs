using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class EventDto : DtoBase
    {
        [Required]
        public string CommonUserId { get; set; }
        [Required]
        public string EventId { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public long? SourceSystemId { get; set; }
        [Required]
        public string Kind { get; set; }
        [Required]
        public string EventType { get; set; }
        public Dictionary<string, object> Properties { get; set; }
    }
}