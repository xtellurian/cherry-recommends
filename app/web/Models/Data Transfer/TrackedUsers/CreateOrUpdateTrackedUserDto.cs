using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable enable
namespace SignalBox.Web.Dto
{
    public class CreateOrUpdateTrackedUserDto : DtoBase
    {
        [Required]
        public string CommonUserId { get; set; } = null!;
        public string? Name { get; set; }
        public Dictionary<string, object>? Properties { get; set; }
        public IntegratedSystemReference? IntegratedSystemReference { get; set; }
    }
}