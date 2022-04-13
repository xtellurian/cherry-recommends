
using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class UpdateChannelPropertiesDto : DtoBase
    {
        public string Endpoint { get; set; }
#nullable enable
        public bool? PopupAskForEmail { get; set; }
    }
}