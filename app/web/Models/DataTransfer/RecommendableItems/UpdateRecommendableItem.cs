using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SignalBox.Core;

namespace SignalBox.Web.Dto
{
#nullable enable
    public class UpdateRecommendableItem : DtoBase
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public double DirectCost { get; set; }
        [Required]
        public BenefitType BenefitType { get; set; }
        [Required]
        [DefaultValue(1d)]
        public double BenefitValue { get; set; }
        [Required]
        public PromotionType PromotionType { get; set; }
        [Required]
        [DefaultValue(1)]
        public int NumberOfRedemptions { get; set; }
        public string? Description { get; set; }
        public DynamicPropertyDictionary? Properties { get; set; }
    }
}