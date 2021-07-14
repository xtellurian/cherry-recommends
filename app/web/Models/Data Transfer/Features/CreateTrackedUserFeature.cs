using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class CreateTrackedUserFeature : DtoBase
    {
        [Required]
        public object Value { get; set; }
    }
}