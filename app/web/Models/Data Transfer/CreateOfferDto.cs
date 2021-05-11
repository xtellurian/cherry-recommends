using System.ComponentModel.DataAnnotations;

#nullable enable
namespace SignalBox.Web.Dto
{
    public class CreateOfferDto : DtoBase
    {
        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string Name { get; set; } = null!;
        public string? Currency { get; set; } = "USD";
        public double Price { get; set; }
        public double? Cost { get; set; }
        public string? DiscountCode { get; set; }
    }
}