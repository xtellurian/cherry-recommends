
using System.ComponentModel.DataAnnotations;
using SignalBox.Core;

namespace SignalBox.Web.Dto
{
    public class CreateChannelDto : DtoBase
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public ChannelTypes ChannelType { get; set; }
        [Required]
        public long IntegratedSystemId { get; set; }
    }
}