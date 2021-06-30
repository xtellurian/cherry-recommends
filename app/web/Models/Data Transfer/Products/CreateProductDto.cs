using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class CreateProductDto : CommonDtoBase
    {
        [Required]
        public double? ListPrice { get; set; }
        public double? DirectCost { get; set; }
        public string Description { get; set; }
    }
}