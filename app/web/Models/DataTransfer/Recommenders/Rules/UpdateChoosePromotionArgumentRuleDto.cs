using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class UpdateChoosePromotionArgumentRuleDto : ArgumentValueRuleDtoBase
    {
        [Required]
        public long PromotionId { get; set; }

    }
}
