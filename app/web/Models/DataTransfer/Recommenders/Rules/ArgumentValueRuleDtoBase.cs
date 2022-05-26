using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public abstract class ArgumentValueRuleDtoBase : DtoBase
    {
        [Required]
        [MinLength(1)]
        [MaxLength(127)]
        public string ArgumentValue { get; set; }
    }
}