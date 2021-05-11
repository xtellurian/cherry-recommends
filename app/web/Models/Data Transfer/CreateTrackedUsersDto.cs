using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class CreateTrackedUsersDto : DtoBase
    {
        [Required]
        public IEnumerable<CreateTrackedUserDto> Users { get; set; }
#nullable enable
        public IEnumerable<EventDto>? Events { get; set; }
    }
}