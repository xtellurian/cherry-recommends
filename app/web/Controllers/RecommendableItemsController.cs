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
            return await workflows.CreateRecommendableItem(dto.CommonId, dto.Name, dto.ListPrice, dto.DirectCost, dto.Description, dto.Properties);
        }

        /// <summary>Updates a recommendable item.</summary>
        [HttpPost("{id}")]
        public async Task<RecommendableItem> Update(string id, UpdateRecommendableItem dto)
        {
            var entity = await store.GetEntity(id);
            return await workflows.UpdateRecommendableItem(entity, dto.Name, dto.ListPrice, dto.DirectCost, dto.Description, dto.Properties);
        }

        protected override async Task<(bool, string)> CanDelete(RecommendableItem entity)
        {
            var isDefaultItem = await workflows.IsDefaultItemForRecommender(entity);
            if (isDefaultItem)
            {
                return (false, "Cannot delete item as it is used as a Recommender's default item");
            }
            else
            {
                return (true, string.Empty);
            }
        }
    }
}