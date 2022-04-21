using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public abstract class CommonDtoBase : DtoBase
    {
        [Required]
        [StringLength(99, MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z0-9_\-|.@]+$", ErrorMessage = "Common Id must only contain alpha-numeric, underscore, hyphen, bar, period or at sign")]
        public string CommonId { get; set; }
        public virtual string Name { get; set; }
    }
}