using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class CreateProductDto : DtoBase
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string ProductId { get; set; }
        public string Description { get; set; }
    }
}