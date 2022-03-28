using System.ComponentModel.DataAnnotations;
using SignalBox.Core;

namespace SignalBox.Web.Dto
{
    public class UpdateWeightDto : DtoBase, IWeighted
    {
        public long Id { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public double Weight { get; set; }
    }
}