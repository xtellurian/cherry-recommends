using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    public class ActivityFeedController : ControllerBase
    {
        private readonly ILogger<ActivityFeedController> logger;
        private readonly IActivityFeedWorkflow workflow;

        public ActivityFeedController(ILogger<ActivityFeedController> logger,
                                        IActivityFeedWorkflow workflow)
        {
            this.logger = logger;
            this.workflow = workflow;
        }

        [HttpGet]
        public async Task<IEnumerable<ActivityFeedEntity>> GetActivityFeedEntities([FromQuery] PaginateRequest p)
        {
            return await workflow.GetActivityFeedEntities(p);
        }
    }
}