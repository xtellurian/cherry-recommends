using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    public class RecommendableItemsController : CommonEntityControllerBase<RecommendableItem>
    {
        private readonly RecommendableItemWorkflows workflows;

        public RecommendableItemsController(IRecommendableItemStore store, RecommendableItemWorkflows workflows) : base(store)
        {
            this.workflows = workflows;
        }

        /// <summary>Creates a new recommendable item.</summary>
        [HttpPost]
        public async Task<RecommendableItem> Create(CreateRecommendableItemDto dto)
        {
            return await workflows.CreateRecommendableItem(dto.CommonId, dto.Name, dto.ListPrice, dto.Description, dto.DirectCost);
        }

        protected override async Task<(bool, string)> CanDelete(RecommendableItem entity)
        {
            await store.LoadMany(entity, _ => _.Recommenders);
            if (entity.Recommenders.Any())
            {
                return (false, "Item is used in a recommender. Delete the recommender first.");
            }
            else
            {
                return (true, null);
            }
        }
    }
}