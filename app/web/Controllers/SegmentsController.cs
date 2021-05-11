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
    public class SegmentsController : ControllerBase
    {
        private readonly ILogger<SegmentsController> logger;
        private readonly SegmentWorkflows workflow;
        private readonly ISegmentStore segmentStore;

        public SegmentsController(ILogger<SegmentsController> logger, SegmentWorkflows workflow, ISegmentStore segmentStore)
        {
            this.logger = logger;
            this.workflow = workflow;
            this.segmentStore = segmentStore;
        }

        [HttpPost]
        public async Task<Segment> CreateSegment([FromBody] CreateSegmentDto dto)
        {
            return await workflow.CreateSegment(dto.Name);
        }

        [HttpGet]
        public async Task<IEnumerable<Segment>> ListSegment()
        {
            return await segmentStore.List();
        }

        [HttpGet("{id}")]
        public async Task<Segment> GetSegment(long id)
        {
            return await segmentStore.Read(id);
        }

        [HttpDelete("{id}")]
        public async Task<object> DeleteSegment(long id)
        {
            var result = await segmentStore.Remove(id);
            return new { success = result };
        }
    }

}
