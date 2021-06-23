using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public abstract class CommonDtoBase : DtoBase
    {
        [Required]
        [StringLength(99, MinimumLength = 3)]
        public string CommonId { get; set; }
        public string Name { get; set; }
    }
}