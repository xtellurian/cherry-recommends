using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class CreateOrUpdateTrackedUserDto : DtoBase
    {
        [Required]
        public string CommonUserId { get; set; }
        public string Name { get; set; }
        public Dictionary<string, object> Properties { get; set; }
    }
}