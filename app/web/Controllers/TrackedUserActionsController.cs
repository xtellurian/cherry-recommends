using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/TrackedUsers")]
    public class TrackedUserActionsController : SignalBoxControllerBase
    {
        private readonly ILogger<TrackedUserActionsController> logger;
        private readonly TrackedUserActionWorkflows actionWorkflows;
        private readonly ITrackedUserStore trackedUserStore;

        public TrackedUserActionsController(ILogger<TrackedUserActionsController> logger,
                                      TrackedUserActionWorkflows actionWorkflows,
                                      ITrackedUserStore trackedUserStore)
        {
            this.logger = logger;
            this.actionWorkflows = actionWorkflows;
            this.trackedUserStore = trackedUserStore;
        }

        /// <summary>Gets the latest actions from a tracked user.</summary>
        [HttpGet("{id}/actions")]
        public async Task<UniqueActionsDto> GetLatestActions(string id, bool? useInternalId = null)
        {
            TrackedUser user;
            if ((useInternalId == null || useInternalId == true) && int.TryParse(id, out var internalId))
            {
                user = await trackedUserStore.Read(internalId);
            }
            else if (useInternalId == true)
            {
                throw new BadRequestException("Internal Ids must be integers");
            }
            else
            {
                user = await trackedUserStore.ReadFromCommonId(id);
            }

            return new UniqueActionsDto(await actionWorkflows.ReadUniqueActionNames(user.CommonId));
        }

        /// <summary>Gets the latest action from a tracked user for this action name.</summary>
        [HttpGet("{id}/actions/{actionName}")]
        public async Task<TrackedUserAction> GetAction(string id, string actionName, bool? useInternalId = null)
        {
            TrackedUser user;
            if ((useInternalId == null || useInternalId == true) && int.TryParse(id, out var internalId))
            {
                user = await trackedUserStore.Read(internalId);
            }
            else if (useInternalId == true)
            {
                throw new BadRequestException("Internal Ids must be integers");
            }
            else
            {
                user = await trackedUserStore.ReadFromCommonId(id);
            }

            return await actionWorkflows.ReadLatestAction(user.CommonId, actionName);
        }
    }
}
