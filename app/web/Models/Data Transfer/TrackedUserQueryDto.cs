using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class TrackedUserQueryDto : DtoBase
    {
        [Required]
        public IEnumerable<string> ExternalIds { get; set; }
    }
}