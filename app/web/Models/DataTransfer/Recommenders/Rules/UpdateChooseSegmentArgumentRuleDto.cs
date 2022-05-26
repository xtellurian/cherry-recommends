using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class UpdateChooseSegmentArgumentRuleDto : ArgumentValueRuleDtoBase
    {
        [Required]
        public long SegmentId { get; set; }

    }
}
