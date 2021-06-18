using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SignalBox.Web.Dto
{
    public class BatchCreateOrUpdateUsersDto : DtoBase
    {
        [Required]
        public IEnumerable<CreateOrUpdateTrackedUserDto> Users { get; set; }
    }
}