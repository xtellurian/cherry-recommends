using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SignalBox.Core;

namespace SignalBox.Web.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class SignalBoxControllerBase : ControllerBase
    {
        protected async Task<TrackedUser> LoadTrackedUser(ITrackedUserStore trackedUserStore,
                                                          string internalOrCommonId,
                                                          bool? useInternalId = null)
        {
            TrackedUser trackedUser;
            if ((useInternalId == null || useInternalId == true) && int.TryParse(internalOrCommonId, out var internalId))
            {
                trackedUser = await trackedUserStore.Read(internalId);
            }
            else if (useInternalId == true)
            {
                throw new BadRequestException("Internal Ids must be integers");
            }
            else
            {
                trackedUser = await trackedUserStore.ReadFromCommonId(internalOrCommonId);
            }

            return trackedUser;
        }
    }
}