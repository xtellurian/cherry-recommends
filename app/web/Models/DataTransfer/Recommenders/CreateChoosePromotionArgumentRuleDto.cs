using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class CreateChoosePromotionArgumentRuleDto : DtoBase
    {
        [Required]
        public long ArgumentId { get; set; }
        [Required]
        public long PromotionId { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(20)]
        public string ArgumentValue { get; set; }
    }
}