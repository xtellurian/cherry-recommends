using System.Collections.Generic;

namespace SignalBox.Web.Dto
{
    public class UniqueActionsDto : DtoBase
    {
        public UniqueActionsDto(IEnumerable<string> actionNames)
        {
            ActionNames = actionNames;
        }

        public IEnumerable<string> ActionNames { get; set; }
    }
}