using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class EventDto : DtoBase
    {
        [Required]
        public string TrackedUserExternalId { get; set; }
        [Required]
        public string Key { get; set; }
#nullable enable
        public string? LogicalValue { get; set; }
        public double? NumericValue { get; set; }
    }
}