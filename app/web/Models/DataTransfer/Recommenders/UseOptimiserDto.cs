using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class UseOptimiserDto : DtoBase
    {
        [Required]
        public bool UseOptimiser { get; set; }
    }
}