using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class CreateSegmentDto : DtoBase
    {
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
    }
}