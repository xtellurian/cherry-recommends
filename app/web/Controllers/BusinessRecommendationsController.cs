using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Recommendations;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/Businesses")]
    public class BusinessRecommendationsController : SignalBoxControllerBase
    {
        private readonly ILogger<BusinessRecommendationsController> logger;
        private readonly IItemsRecommendationStore itemsRecommendationStore;
        private readonly IBusinessStore businessStore;

        public BusinessRecommendationsController(ILogger<BusinessRecommendationsController> logger,
                                                 IItemsRecommendationStore itemsRecommendationStore,
                                                 IBusinessStore businessStore)
        {
            this.logger = logger;
            this.itemsRecommendationStore = itemsRecommendationStore;
            this.businessStore = businessStore;
        }

        /// <summary>Gets recent recommendations for a business.</summary>
        [HttpGet("{id}/recommendations")]
        public async Task<Paginated<ItemsRecommendation>> GetRecommendations([FromQuery] PaginateRequest p, string id, bool? useInternalId = null)
        {
            var business = await businessStore.GetEntity(id, useInternalId);
            return await itemsRecommendationStore.Query(new EntityStoreQueryOptions<ItemsRecommendation>(p.Page, _ => _.BusinessId == business.Id));
        }
    }
}
