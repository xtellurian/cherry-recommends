using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class CreateEnvironment : DtoBase
    {
        [Required]
        [StringLength(28, MinimumLength = 3)]
        public string Name { get; set; }
    }
}