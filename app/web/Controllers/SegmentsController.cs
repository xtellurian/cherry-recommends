using System.Collections.Generic;
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
    public class SegmentsController : EntityControllerBase<SignalBox.Core.Segment>
    {
        private readonly ILogger<SegmentsController> logger;
        private readonly CustomerSegmentWorkflows workflow;

        public SegmentsController(ILogger<SegmentsController> logger, CustomerSegmentWorkflows workflow, ISegmentStore store): base(store)
        {
            this.logger = logger;
            this.workflow = workflow;
        }

        /// <summary>Creates a new segment.</summary>
        [HttpPost]
        public async Task<SignalBox.Core.Segment> CreateSegment([FromBody] CreateSegmentDto dto)
        {
            return await workflow.CreateSegment(dto.Name);
        }
    }
}
