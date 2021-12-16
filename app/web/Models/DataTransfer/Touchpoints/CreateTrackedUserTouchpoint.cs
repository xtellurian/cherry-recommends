using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class CreateTrackedUserTouchpoint : DtoBase
    {
        [Required]
        public Dictionary<string, object> Values { get; set; }
    }
}