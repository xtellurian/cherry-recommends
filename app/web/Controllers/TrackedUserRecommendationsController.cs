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
    [Route("api/TrackedUsers")]
    public class TrackedUserRecommendationsController : SignalBoxControllerBase
    {
        private readonly ILogger<TrackedUserRecommendationsController> logger;
        private readonly IItemsRecommendationStore itemsRecommendationStore;
        private readonly IParameterSetRecommendationStore parameterSetRecommendationStore;
        private readonly ITrackedUserStore trackedUserStore;

        public TrackedUserRecommendationsController(ILogger<TrackedUserRecommendationsController> logger,
        IItemsRecommendationStore itemsRecommendationStore,
        IParameterSetRecommendationStore parameterSetRecommendationStore,
                                      ITrackedUserStore trackedUserStore)
        {
            this.logger = logger;
            this.itemsRecommendationStore = itemsRecommendationStore;
            this.parameterSetRecommendationStore = parameterSetRecommendationStore;
            this.trackedUserStore = trackedUserStore;
        }

        /// <summary>Gets recent recommendations for a tracked user.</summary>
        [HttpGet("{id}/latest-recommendations")]
        public async Task<Paginated<object>> GetRecommendations([FromQuery] PaginateRequest p, string id, bool? useInternalId = null)
        {
            var trackedUser = await trackedUserStore.GetEntity(id);
            var parameterSetRecommendations = await parameterSetRecommendationStore.Query(1, _ => _.TrackedUserId == trackedUser.Id);
            var itemsRecommendations = await itemsRecommendationStore.Query(1, _ => _.TrackedUserId == trackedUser.Id);
            var recommendations = new List<object>();
            recommendations.AddRange(parameterSetRecommendations.Items);
            recommendations.AddRange(itemsRecommendations.Items);
            var result = new Paginated<object>(recommendations, 1, recommendations.Count, 1);
            return result;
        }
    }
}
