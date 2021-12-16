using System;
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
    [Route("api/Customers")]
    public class TrackedUserActionsController : SignalBoxControllerBase
    {
        private readonly ILogger<TrackedUserActionsController> logger;

        public TrackedUserActionsController(ILogger<TrackedUserActionsController> logger)
        {
            this.logger = logger;
        }

        /// <summary>Gets the latest actions from a tracked user.</summary>
        [HttpGet("{id}/action-groups")]
        [Obsolete]
        public Paginated<ActionCategoryAndName> GetActionGroups([FromQuery] PaginateRequest p, string id, bool? useInternalId = null)
        {
            logger.LogWarning("This action is deprecated");
            return new Paginated<ActionCategoryAndName>(Enumerable.Empty<ActionCategoryAndName>(), 0, 0, 0);
        }

        /// <summary>Gets the latest action from a tracked user for this category.</summary>
        [HttpGet("{id}/actions/{category}")]
        public object GetAction(string id, string category, string actionName = null, bool? useInternalId = null)
        {
            logger.LogWarning("This action is deprecated");
            return new object();
        }

        /// <summary>Gets actions for a tracked user.</summary>
        [HttpGet("{id}/actions")]
        public Paginated<TrackedUserAction> GetUserActions([FromQuery] PaginateRequest p, string id, bool? revenueOnly = null, bool? useInternalId = null)
        {
            logger.LogWarning("This action is deprecated");
            return new Paginated<TrackedUserAction>(Enumerable.Empty<TrackedUserAction>(), 0, 0, 0);
        }
    }
}
