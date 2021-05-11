using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class CreateTrackedUserDto : DtoBase
    {
        [Required]
        public string ExternalId { get; set; }
        public string Name { get; set; }
    }
}