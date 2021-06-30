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
    [Route("api/[controller]")]
    public class TouchpointsController : CommonEntityControllerBase<Touchpoint>
    {
        private readonly ILogger<TouchpointsController> logger;
        private readonly TouchpointWorkflows workflow;

        public TouchpointsController(ILogger<TouchpointsController> logger, TouchpointWorkflows workflow, ITouchpointStore store) : base(store)
        {
            this.logger = logger;
            this.workflow = workflow;
        }

        /// <summary>Creates a new generic touchpoint that can used on any tracked user.</summary>
        [HttpPost]
        public async Task<Touchpoint> CreateTouchpointMetadata([FromBody] CreateTouchpointMetadata dto)
        {
            return await workflow.CreateTouchpoint(dto.CommonId, dto.Name);
        }

        /// <summary>Creates a new generic touchpoint that can used on any tracked user.</summary>
        [HttpGet("{id}/TrackedUsers")]
        public async Task<Paginated<TrackedUser>> GetTouchpointUsers(string id, [FromQuery] PaginateRequest p)
        {
            var touchpoint = await base.GetResource(id);
            return await workflow.GetTrackedUsers(touchpoint, p.Page);
        }
    }
}
