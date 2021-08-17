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
        [HttpGet("{id}/action-groups")]
        public async Task<Paginated<ActionCategoryAndName>> GetActionGroups([FromQuery] PaginateRequest p, string id, bool? useInternalId = null)
        {
            var trackedUser = await trackedUserStore.GetEntity(id);
            return await actionWorkflows.ReadTrackedUserCategoriesAndActionNames(p.Page, trackedUser.CommonId);
        }

        /// <summary>Gets the latest action from a tracked user for this category.</summary>
        [HttpGet("{id}/actions/{category}")]
        public async Task<TrackedUserAction> GetAction(string id, string category, string actionName = null, bool? useInternalId = null)
        {
            var user = await trackedUserStore.GetEntity(id, useInternalId);

            return await actionWorkflows.ReadLatestAction(user.CommonId, category, actionName);
        }
    }
}
