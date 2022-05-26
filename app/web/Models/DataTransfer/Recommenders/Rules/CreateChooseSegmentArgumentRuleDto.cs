using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class CreateChooseSegmentArgumentRuleDto : ArgumentValueRuleDtoBase
    {
        [Required]
        public long ArgumentId { get; set; }
        [Required]
        public long SegmentId { get; set; }
    }
}
