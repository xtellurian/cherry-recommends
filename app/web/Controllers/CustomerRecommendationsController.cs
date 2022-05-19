using System.Collections.Generic;
using System.Linq;
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
    [Route("api/TrackedUsers")]
    [Route("api/Customers")]
    public class CustomerRecommendationsController : SignalBoxControllerBase
    {
        private readonly IItemsRecommendationStore itemsRecommendationStore;
        private readonly IParameterSetRecommendationStore parameterSetRecommendationStore;
        private readonly ICustomerStore customerStore;

        public CustomerRecommendationsController(IItemsRecommendationStore itemsRecommendationStore,
                                                 IParameterSetRecommendationStore parameterSetRecommendationStore,
                                                 ICustomerStore customerStore)
        {
            this.itemsRecommendationStore = itemsRecommendationStore;
            this.parameterSetRecommendationStore = parameterSetRecommendationStore;
            this.customerStore = customerStore;
        }

        /// <summary>Gets recent recommendations for a Customer.</summary>
        [HttpGet("{id}/latest-recommendations")]
        public async Task<Paginated<object>> GetRecommendations([FromQuery] PaginateRequest p, string id, bool? useInternalId = null)
        {
            var customer = await customerStore.GetEntity(id, useInternalId);
            var parameterSetRecommendations = await parameterSetRecommendationStore.QueryForCustomer(p, customer.Id);
            var itemsRecommendations = await itemsRecommendationStore.QueryForCustomer(p, customer.Id);
            var recommendations = new List<RecommendationEntity>();
            recommendations.AddRange(parameterSetRecommendations.Items);
            recommendations.AddRange(itemsRecommendations.Items);
            var toSerializeProperly = new List<object>(recommendations.OrderByDescending(_ => _.Created));
            var result = new Paginated<object>(toSerializeProperly, 1, toSerializeProperly.Count, 1);
            return result;
        }
    }
}
