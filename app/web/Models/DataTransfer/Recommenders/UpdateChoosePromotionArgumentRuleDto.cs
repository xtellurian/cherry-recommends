using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class UpdateChoosePromotionArgumentRuleDto : DtoBase
    {
        [Required]
        public long PromotionId { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(20)]
        public string ArgumentValue { get; set; }
    }
}