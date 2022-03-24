using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class UpdateWeightDto : DtoBase
    {
        [Required]
        public double? Weight { get; set; }
    }
}