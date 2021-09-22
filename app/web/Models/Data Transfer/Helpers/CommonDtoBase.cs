using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public abstract class CommonDtoBase : DtoBase
    {
        [Required]
        [StringLength(99, MinimumLength = 3)]
        [RegularExpression(@"^[a-z0-9_\-]+$", ErrorMessage = "Common Id can only contain lowercase letters, numbers, underscore, and hyphen.")]
        public string CommonId { get; set; }
        public virtual string Name { get; set; }
    }
}