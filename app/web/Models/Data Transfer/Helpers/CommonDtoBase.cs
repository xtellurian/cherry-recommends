using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public abstract class CommonDtoBase : DtoBase
    {
        [Required]
        [StringLength(99, MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z0-9_\-]+$", ErrorMessage = "Common Id can only contain alphanumeric chars, underscores, and hyphens.")]
        public string CommonId { get; set; }
        public virtual string Name { get; set; }
    }
}