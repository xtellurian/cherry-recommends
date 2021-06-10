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
    [Route("api/trackedusers")]
    public class TrackedUserTouchpointsController : SignalBoxControllerBase
    {
        private readonly ILogger<TrackedUserTouchpointsController> _logger;
        private readonly TouchpointWorkflows workflows;
        private readonly ITrackedUserStore trackedUserStore;
        private readonly ITouchpointStore touchpointStore;
        private readonly ITrackedUserTouchpointStore trackedUserTouchpointStore;

        public TrackedUserTouchpointsController(ILogger<TrackedUserTouchpointsController> logger,
                                      TouchpointWorkflows workflows,
                                      ITrackedUserStore trackedUserStore,
                                      ITouchpointStore touchpointStore,
                                      ITrackedUserTouchpointStore trackedUserTouchpointStore)
        {
            _logger = logger;
            this.workflows = workflows;
            this.trackedUserStore = trackedUserStore;
            this.touchpointStore = touchpointStore;
            this.trackedUserTouchpointStore = trackedUserTouchpointStore;
        }

        /// <summary>Returns a list of touchpoints available for a tracked user.</summary>
        [HttpGet("{id}/touchpoints")]
        public async Task<IEnumerable<Touchpoint>> AvailableTouchpoints(string id, bool? useInternalId = null)
        {
            var trackedUser = await LoadTrackedUser(trackedUserStore, id, useInternalId);
            return await trackedUserTouchpointStore.GetTouchpointsFor(trackedUser);
        }

        /// <summary>Creates a new set of touchpoint values on a user. You probably shouldn't set this manually..</summary>
        [HttpPost("{id}/touchpoints/{touchpointCommonId}")]
        public async Task<TrackedUserTouchpoint> Create(string id,
                                         string touchpointCommonId,
                                         [FromBody] CreateTrackedUserTouchpoint dto,
                                         [FromQuery] bool? useInternalId = null)
        {
            var trackedUser = await LoadTrackedUser(trackedUserStore, id, useInternalId);
            return await workflows.CreateTouchpointOnUser(trackedUser, touchpointCommonId, dto.Values);
        }

        /// <summary>Returns the values set in the touchpoint.</summary>
        [HttpGet("{id}/touchpoints/{touchpointCommonId}")]
        public async Task<TrackedUserTouchpoint> ReadTouchpointValues(string id,
                                         string touchpointCommonId,
                                         [FromQuery] bool? useInternalId = null,
                                         [FromQuery] int? version = null)
        {
            var trackedUser = await LoadTrackedUser(trackedUserStore, id, useInternalId);
            return await workflows.ReadTouchpointValues(trackedUser, touchpointCommonId, version);
        }
    }
}
