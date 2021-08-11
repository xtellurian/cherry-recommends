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
    [Route("api/Actions")]
    public class ActionsController : SignalBoxControllerBase
    {
        private readonly ILogger<ActionsController> logger;
        private readonly IRewardSelectorStore rewardSelectorStore;
        private readonly ITrackedUserActionStore actionStore;

        public ActionsController(ILogger<ActionsController> logger,
                                IRewardSelectorStore rewardSelectorStore,
                                ITrackedUserActionStore actionStore)
        {
            this.logger = logger;
            this.rewardSelectorStore = rewardSelectorStore;
            this.actionStore = actionStore;
        }

        /// <summary>Gets unique action names.</summary>
        [HttpGet("distinct-names")]
        public async Task<Paginated<string>> GetUniqueActions([FromQuery] PaginateRequest p, string term)
        {
            return await actionStore.ReadAllUniqueActionNames(p.Page, term);
        }
        
        /// <summary>Gets unique action names.</summary>
        [HttpGet("distinct-groups")]
        public async Task<Paginated<ActionCategoryAndName>> GetDistinctGroups([FromQuery] PaginateRequest p, string term)
        {
            return await actionStore.ReadAllCategoriesWithActionNames(p.Page, term);
        }
    }
}
