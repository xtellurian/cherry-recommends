using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class CreateChoosePromotionArgumentRuleDto : ArgumentValueRuleDtoBase
    {
        [Required]
        public long ArgumentId { get; set; }
        [Required]
        public long PromotionId { get; set; }
    }
}