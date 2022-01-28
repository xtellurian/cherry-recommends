using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class CreateCustomerMetric : DtoBase
    {
        [Required]
        public object Value { get; set; }
    }
}